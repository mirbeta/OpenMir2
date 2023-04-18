using GameSrv.Actor;
using GameSrv.GameCommand;
using GameSrv.Player;
using GameSrv.Script;
using GameSrv.ScriptSystem;
using SystemModule.Enums;

namespace GameSrv
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

        private delegate void HandleGrobalMessage(PlayObject PlayObject, string sVariable, ref string sMsg);

        /// <summary>
        /// 初始化全局变量脚本处理列表
        /// </summary>
        public GrobalVarProcessingSys()
        {
            ProcessGrobalMessage = new Dictionary<int, HandleGrobalMessage>();
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_SERVERNAME] = GetServerName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_SERVERIP] = GetServerIp;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_WEBSITE] = GetWebSite;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BBSSITE] = GetBbsWeiSite;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CLIENTDOWNLOAD] = GetCilentDownLoad;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_QQ] = GetQQ;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_PHONE] = GetPhone;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT0] = GetBankAccount0;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT1] = GetBankAccount1;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT2] = GetBankAccount2;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT3] = GetBankAccount3;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT4] = GetBankAccount4;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT5] = GetBankAccount5;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT6] = GetBankAccount6;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT7] = GetBankAccount7;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT8] = GetBankAccount8;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BANKACCOUNT9] = GetBankAccount9;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GAMEGOLDNAME] = GetGameGoldName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GAMEPOINTNAME] = GetPointName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_USERCOUNT] = GetUserCount;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_DATETIME] = GetDateTime;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_USERNAME] = GetUserName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAPNAME] = GetMapName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDNAME] = GetGuilidName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_RANKNAME] = GetGuilidRankName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_LEVEL] = GetLevel;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_HP] = GetHP;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXHP] = GetMaxHP;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MP] = GetMP;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXMP] = GetMaxHP;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_AC] = GetAc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXAC] = GetMaxAc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAC] = GetMac;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXMAC] = GetMaxMac;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_DC] = GetDC;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXDC] = GetMaxDC;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MC] = GetMc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXMC] = GetMaxMc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_SC] = GetSc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXSC] = GetMaxSc;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_EXP] = GetExp;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_MAXEXP] = GetMaxExp;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_PKPOINT] = GetPkPoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CREDITPOINT] = GetCreditPoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GOLDCOUNT] = GetGoldCount;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GAMEGOLD] = GetGameGold;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GAMEPOINT] = GetGamePoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_LOGINTIME] = GetLoginTime;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_LOGINLONG] = GetLoginTime;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_DRESS] = GetDress;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_WEAPON] = GetWeapon;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_RIGHTHAND] = GetRightHand;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_HELMET] = GetHelmet;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_NECKLACE] = GetNecklace;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_RING_R] = GetRing_R;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_RING_L] = GetRing_L;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_ARMRING_R] = GetArmring_R;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_ARMRING_L] = GetArmring_L;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BUJUK] = GetBujuk;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BELT] = GetBelt;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BOOTS] = GetBoots;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CHARM] = GetChrm;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_IPADDR] = GetIpAddr;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_IPLOCAL] = GetIpLocal;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDBUILDPOINT] = GeTGuildBuildPoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDAURAEPOINT] = GeTGuildAuraePoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDSTABILITYPOINT] = GeTGuildStabilityPoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDFLOURISHPOINT] = GeTGuildFlourishPoint;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_REQUESTCASTLEWARITEM] = GetRequestCastlewarItem;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_REQUESTCASTLEWARDAY] = GetRequestCastleWarday;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_REQUESTBUILDGUILDITEM] = GetRequestBuildGuildItem;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_OWNERGUILD] = GetOwnerGuild;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CASTLENAME] = GetCastleName;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_LORD] = GetLord;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_GUILDWARFEE] = GeTGuildWarfee;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_BUILDGUILDFEE] = GetBuildGuildfee;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CASTLEWARDATE] = GetCastleWarDate;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_LISTOFWAR] = GetListofWar;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CASTLECHANGEDATE] = GetCastleChangeDate;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CASTLEWARLASTDATE] = GetCastlewarLastDate;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CASTLEGETDAYS] = GetCastlegetDays;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_DATE] = GetCmdDate;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_ALLOWMSG] = GetCmdAllowmsg;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_LETSHOUT] = GetCmdletshout;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_LETTRADE] = GetCmdLettrade;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_LETGuild] = GetCmdLeTGuild;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_ENDGUILD] = GetCmdEndGuild;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_BANGUILDCHAT] = GetCmdBanGuildChat;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_AUTHALLY] = GetCmdAuthally;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_AUTH] = GetCmdAuth;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_AUTHCANCEL] = GetCmdAythCcancel;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_USERMOVE] = GetCmdUserMove;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_SEARCHING] = GetCmdSearching;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_ALLOWGROUPCALL] = GetCmdAllowGroupCall;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_CMD_GROUPRECALLL] = GetCmdGroupCall;
            ProcessGrobalMessage[GrobalVarCodeDef.nVAR_STR] = GetGrobalVarStr;
        }

        /// <summary>
        /// 处理脚本
        /// </summary>
        /// <param name="nIdx"></param>
        public static void Handler(PlayObject PlayObject, int nIdx, string sVariable, ref string sMsg)
        {
            if (ProcessGrobalMessage.ContainsKey(nIdx))
            {
                ProcessGrobalMessage[nIdx](PlayObject, sVariable, ref sMsg);
                //if (nIdx < FProcessGrobalMessage.Count())
                //{
                //    FProcessGrobalMessage[nIdx](PlayObject, sVariable, ref sMsg);
                //}
                //else
                //{
                //    M2Share.Logger.Error(string.Format("未知全局变量:{0}", nIdx));
                //}
            }
        }

        internal void GetGrobalVarStr(PlayObject PlayObject, string sVariable, ref string sMsg)
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
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNVal[n18 - 1100]);
                }
                else if (HUtil32.RangeInDefined(n18, 1110, 1119))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MDyVal[n18 - 1110]);
                }
                else if (HUtil32.RangeInDefined(n18, 1200, 1299))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNMval[n18 - 1200]);
                }
                else if (HUtil32.RangeInDefined(n18, 1000, 1099))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobaDyMval[n18 - 1000]);
                }
                else if (HUtil32.RangeInDefined(n18, 1300, 1399))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNInteger[n18 - 1300]);
                }
                else if (HUtil32.RangeInDefined(n18, 1400, 1499))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MSString[n18 - 1400]);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetUserCount(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", Convert.ToString(M2Share.WorldEngine.PlayObjectCount));
        }

        /// <summary>
        /// 取服务器名称
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.ServerName);
        }

        /// <summary>
        /// 取网站地址
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetWebSite(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.sWebSite);
        }

        /// <summary>
        /// 取服务器时间
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDateTime(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", DateTime.Now.ToString("dddddd,dddd,hh:mm:nn"));
        }

        /// <summary>
        /// 取玩家名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetUserName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", PlayObject.ChrName);
        }

        /// <summary>
        /// 取行会名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGuilidName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            if (PlayObject.MyGuild != null)
            {
                sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), PlayObject.MyGuild.GuildName);
            }
            else
            {
                sMsg = "无";
            }
        }

        /// <summary>
        /// 取玩行会封号名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGuilidRankName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", PlayObject.GuildRankName);
        }

        /// <summary>
        /// 查看申请攻城战役行会列表
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetListofWar(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            if (PlayObject.Castle != null)
            {
                sMsg = PlayObject.Castle.GetAttackWarList();
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleWarDate(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            if (PlayObject.Castle == null)
            {
                PlayObject.Castle = M2Share.CastleMgr.GetCastle(0);
            }
            if (PlayObject.Castle != null)
            {
                if (!PlayObject.Castle.UnderWar)
                {
                    sMsg = PlayObject.Castle.GetWarDate();
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDealGoldPlay(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.GetPoseCreate() == PlayObject) && (PoseHuman.Race == ActorRace.Play))
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerRunTime(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime).ToString("YYYY-MM-DD HH:mm:ss"));
        }

        /// <summary>
        /// 取服务器运行天数
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMacrunTime(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, string.Format("<{0}>"), Convert.ToString(HUtil32.GetTickCount() / (24 * 60 * 60 * 1000)));
        }

        /// <summary>
        /// 取最高等级人物信息
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighLevelInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highLevelPlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighPkInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highPvpPlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighDcInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highDcPlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighMcInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highMcPlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighScInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highScPlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighOlineInfo(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highOnlinePlay = M2Share.ActorMgr.Get<PlayObject>(M2Share.HighLevelHuman);
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
        internal void GetLoginLong(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = ((HUtil32.GetTickCount() - PlayObject.LogonTick) / 60000) + "分钟";
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 取玩家登录时间
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLoginTime(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", Convert.ToString(PlayObject.LogonTime));
        }

        /// <summary>
        /// 取行会建筑度
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildBuildPoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (PlayObject.MyGuild.BuildPoint).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 行会人气度
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildAuraePoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (PlayObject.MyGuild.Aurae).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取行会安定度
        /// </summary>
        internal void GeTGuildStabilityPoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (PlayObject.MyGuild.Stability).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取行会繁荣度
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildFlourishPoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (PlayObject.MyGuild.Flourishing).ToString();
            }
            sMsg = CombineStr(sMsg, string.Format("<{0}>", sVariable), sText);
        }

        /// <summary>
        /// 取攻城需要的物品
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestCastlewarItem(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.ZumaPiece);
        }

        /// <summary>
        /// 取城保所属行会
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetOwnerGuild(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                sText = PlayObject.Castle.OwnGuild;
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                sText = PlayObject.Castle.sName;
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLord(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                if (PlayObject.Castle.MasterGuild != null)
                {
                    sText = PlayObject.Castle.MasterGuild.GetChiefName();
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleChangeDate(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                sText = Convert.ToString(PlayObject.Castle.ChangeDate);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastlewarLastDate(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                sText = Convert.ToString(PlayObject.Castle.WarDate);
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastlegetDays(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (PlayObject.Castle != null)
            {
                sText = (HUtil32.GetDayCount(DateTime.Now, PlayObject.Castle.ChangeDate)).ToString();
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
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMapName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", PlayObject.Envir.MapDesc);
        }

        /// <summary>
        /// 取地图文件名
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMapFileName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", PlayObject.Envir.MapName);
        }

        /// <summary>
        /// 取当前对象等级
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLevel(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.Abil.Level).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象血量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHP(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = Convert.ToString(PlayObject.WAbil.HP);
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象最大血量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxHP(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.WAbil.MaxHP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象魔法值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMP(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.WAbil.MP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象最大魔法值
        /// </summary>
        internal void GetMaxMP(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.WAbil.MaxMP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象攻击力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDC(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(PlayObject.WAbil.DC)).ToString();
            sMsg = CombineStr(sMsg, "<$DC>", sText);
        }

        /// <summary>
        /// 取当前对象最大攻击力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxDC(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(PlayObject.WAbil.DC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXDC>", sText);
        }

        /// <summary>
        /// 取当前对象魔法防御
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMac(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(PlayObject.WAbil.MAC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAC>", sText);
        }

        /// <summary>
        /// 取当前对象魔法力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(PlayObject.WAbil.MC)).ToString();
            sMsg = CombineStr(sMsg, "<$MC>", sText);
        }

        /// <summary>
        /// 取当前对象最大魔法力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxMc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(PlayObject.WAbil.MC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXMC>", sText);
        }

        /// <summary>
        /// 取当前对象道术力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetSc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(PlayObject.WAbil.SC)).ToString();
            sMsg = CombineStr(sMsg, "<$SC>", sText);
        }

        /// <summary>
        /// 取当前对象最大道术力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxSc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(PlayObject.WAbil.SC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXSC>", sText);
        }

        /// <summary>
        /// 取当前对象经验值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetExp(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.Abil.Exp).ToString();
            sMsg = CombineStr(sMsg, "<$EXP>", sText);
        }

        /// <summary>
        /// 最当前对象最大经验值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxExp(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.Abil.MaxExp).ToString();
            sMsg = CombineStr(sMsg, "<$MAXEXP>", sText);
        }

        /// <summary>
        /// 取当前最新最大魔法防御力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxMac(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(PlayObject.WAbil.MAC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXMAC>", sText);
        }

        /// <summary>
        /// 取当前对象最大防御值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxAc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXAC>", HUtil32.HiWord(PlayObject.WAbil.AC));
        }

        /// <summary>
        /// 取当前对象负重力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHw(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$HW>", PlayObject.WAbil.HandWeight);
        }

        /// <summary>
        /// 取当前对象防御
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetAc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$AC>", HUtil32.LoWord(PlayObject.WAbil.AC));
        }

        /// <summary>
        /// 取当前对象最大负重力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxHw(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXHW>", PlayObject.WAbil.MaxHandWeight);
        }

        /// <summary>
        /// 取当前对象腕力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetWW(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$WW>", PlayObject.WAbil.WearWeight);
        }

        /// <summary>
        /// 取当前对象最大腕力
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxWW(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXWW>", PlayObject.WAbil.MaxWearWeight);
        }

        /// <summary>
        /// 取当前对象包裹重量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBw(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BW>", PlayObject.WAbil.Weight);
        }

        /// <summary>
        /// 取当前对象包裹最大重量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxBw(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXBW>", PlayObject.WAbil.MaxWeight);
        }

        /// <summary>
        /// 取当前对象怪物经验值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMonExp(AnimalObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MONEXP>", PlayObject.FightExp);
        }

        /// <summary>
        /// 取当前对象PK点
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPkPoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$PKPOINT>", PlayObject.PkPoint);
        }

        /// <summary>
        /// 取当前对象金币数量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGoldCount(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = Convert.ToString(PlayObject.Gold + '/' + PlayObject.GoldMax);
            sMsg = CombineStr(sMsg, "<$GOLDCOUNT>", sText);
        }

        /// <summary>
        /// 取当前对象金币数量
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGameGold(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.GameGold).ToString();
            sMsg = CombineStr(sMsg, "<$GAMEGOLD>", sText);
        }

        /// <summary>
        /// 取当前对象声望点
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGamePoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.GamePoint).ToString();
            sMsg = CombineStr(sMsg, "<$GAMEPOINT>", sText);
        }

        /// <summary>
        /// 当前对象声望点数
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCreditPoint(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.CreditPoint).ToString();
            sMsg = CombineStr(sMsg, "<$CREDITPOINT>", sText);
        }

        /// <summary>
        /// 取金币名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGameGoldName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GAMEGOLDNAME>", M2Share.Config.GameGoldName);
        }

        /// <summary>
        /// 取当前对象饥饿程度
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHunGer(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.GetMyStatus()).ToString();
            sMsg = CombineStr(sMsg, "<$HUNGER>", sText);
        }

        /// <summary>
        /// 取当前对象所在地图X坐标
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCurrX(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.CurrX).ToString();
            sMsg = CombineStr(sMsg, "<$CURRX>", sText);
        }

        /// <summary>
        /// 取当前对象所在地图X坐标
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCurrY(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.CurrY).ToString();
            sMsg = CombineStr(sMsg, "<$CURRX>", sText);
        }

        /// <summary>
        /// 取声望点名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPointName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GAMEPOINTNAME>", M2Share.Config.GamePointName);
        }

        /// <summary>
        /// 取当前对象所在IP地址
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetIpAddr(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$IPADDR>", PlayObject.LoginIpAddr);
        }

        /// <summary>
        /// 取当前对象所在区域
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetIpLocal(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            //string sText = PlayObject.m_sIPLocal;
            // GetIPLocal(PlayObject.m_sIPaddr);
            sMsg = CombineStr(sMsg, "<$IPLOCAL>", "无");
        }

        /// <summary>
        /// 行会战金币数
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildWarfee(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GUILDWARFEE>", (M2Share.Config.GuildWarPrice).ToString());
        }
        
        /// <summary>
        /// 建立行会所需的金币数
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBuildGuildfee(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BUILDGUILDFEE>", (M2Share.Config.BuildGuildPrice).ToString());
        }

        /// <summary>
        /// 允许建立行会的物品名字
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestBuildGuildItem(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$REQUESTBUILDGUILDITEM>", M2Share.Config.WomaHorn);
        }
        
        /// <summary>
        /// 多少天后攻城
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestCastleWarday(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$REQUESTCASTLEWARDAY>", M2Share.Config.ZumaPiece);
        }

        /// <summary>
        /// 取日期命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdDate(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_DATE>", CommandMgr.GameCommands.Data.CmdName);
        }

        /// <summary>
        /// 查看接收所有消息命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAllowmsg(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_ALLOWMSG>", CommandMgr.GameCommands.AllowMsg.CmdName);
        }

        /// <summary>
        /// 查看允许群聊命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdletshout(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_LETSHOUT>", CommandMgr.GameCommands.Letshout.CmdName);
        }

        /// <summary>
        /// 查看允许交易命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdLettrade(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_LETTRADE>", CommandMgr.GameCommands.LetTrade.CmdName);
        }

        /// <summary>
        /// 查看允许加入门派命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdLeTGuild(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_LETGuild>", CommandMgr.GameCommands.LetGuild.CmdName);
        }

        /// <summary>
        /// 查看退出门派命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdEndGuild(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_ENDGUILD>", CommandMgr.GameCommands.EndGuild.CmdName);
        }

        /// <summary>
        /// 查看允许行会聊天命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdBanGuildChat(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_BANGUILDCHAT>", CommandMgr.GameCommands.BanGuildChat.CmdName);
        }

        /// <summary>
        /// 查看允许行会联盟命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAuthally(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_AUTHALLY>", CommandMgr.GameCommands.Authally.CmdName);
        }

        /// <summary>
        /// 查看行会联盟命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAuth(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_AUTH>", CommandMgr.GameCommands.Auth.CmdName);
        }

        /// <summary>
        /// 查看取消行会联盟命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAythCcancel(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_AUTHCANCEL>", CommandMgr.GameCommands.AuthCancel.CmdName);
        }

        /// <summary>
        /// 查看传送戒指命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdUserMove(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_USERMOVE>", CommandMgr.GameCommands.UserMove.CmdName);
        }

        /// <summary>
        /// 查看探测项链命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdSearching(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_SEARCHING>", CommandMgr.GameCommands.Searching.CmdName);
        }

        /// <summary>
        /// 查看组队传送命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdGroupCall(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_GROUPRECALLL>", CommandMgr.GameCommands.GroupRecalll.CmdName);
        }

        /// <summary>
        /// 查看允许组队传送命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAllowGroupCall(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_ALLOWGROUPCALL>", CommandMgr.GameCommands.AllowGroupCall.CmdName);
        }

        /// <summary>
        /// 查看更改仓库密码命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageChgPassword(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_STORAGECHGPASSWORD>", CommandMgr.GameCommands.ChgPassword.CmdName);
        }

        /// <summary>
        /// 为仓库设定一个4-7位数长的仓库密码命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageSetPassword(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_STORAGESETPASSWORD>", CommandMgr.GameCommands.SetPassword.CmdName);
        }

        /// <summary>
        /// 可将仓库锁定命令
        /// </summary>
        internal void GetCmdStorageLock(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_STORAGELOCK>", CommandMgr.GameCommands.Lock.CmdName);
        }

        /// <summary>
        /// 开启仓库命令
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageUnlock(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CMD_STORAGEUNLOCK>", CommandMgr.GameCommands.UnlockStorage.CmdName);
        }

        /// <summary>
        /// 取当前对象武器名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetWeapon(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Weapon].Index);
            sMsg = CombineStr(sMsg, "<$WEAPON>", sText);
        }

        /// <summary>
        /// 取当前对象衣服名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetDress(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Dress].Index);
            sMsg = CombineStr(sMsg, "<$DRESS>", sText);
        }

        /// <summary>
        /// 取当前对象蜡烛名称(勋章)
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetRightHand(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.RighThand].Index);
            sMsg = CombineStr(sMsg, "<$RIGHTHAND>", sText);
        }

        /// <summary>
        /// 取当前对象头盔名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetHelmet(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Helmet].Index);
            sMsg = CombineStr(sMsg, "<$HELMET>", sText);
        }

        /// <summary>
        /// 取当前对象项链名称
        /// </summary>
        internal unsafe void GetNecklace(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Necklace].Index);
            sMsg = CombineStr(sMsg, "<$NECKLACE>", sText);
        }

        /// <summary>
        /// 取当前对象右戒指名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetRing_R(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringr].Index);
            sMsg = CombineStr(sMsg, "<$RING_R>", sText);
        }

        /// <summary>
        /// 取当前对象左戒指名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetRing_L(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
           string  sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringl].Index);
            sMsg = CombineStr(sMsg, "<$RING_L>", sText);
        }

        /// <summary>
        /// 取当前对象右手镯名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetArmring_R(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingr].Index);
            sMsg = CombineStr(sMsg, "<$ARMRING_R>", sText);
        }

        /// <summary>
        /// 取当前对象左手镯名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetArmring_L(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingl].Index);
            sMsg = CombineStr(sMsg, "<$ARMRING_L>", sText);
        }

        /// <summary>
        /// 取当前对象护身符名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetBujuk(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Bujuk].Index);
            sMsg = CombineStr(sMsg, "<$BUJUK>", sText);
        }

        /// <summary>
        /// 取当前对象腰带名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetBelt(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Belt].Index);
            sMsg = CombineStr(sMsg, "<$BELT>", sText);
        }

        /// <summary>
        /// 取当前对象鞋子名称 
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetBoots(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Boots].Index);
            sMsg = CombineStr(sMsg, "<$BOOTS>", sText);
        }

        /// <summary>
        /// 取当前对象宝石名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal unsafe void GetChrm(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Charm].Index);
            sMsg = CombineStr(sMsg, "<$CHARM>", sText);
        }

        /// <summary>
        /// 取客服QQ
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetQQ(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$QQ>", M2Share.Config.sQQ);
        }

        /// <summary>
        /// 取客服手机号码
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPhone(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$PHONE>", M2Share.Config.sPhone);
        }

        /// <summary>
        /// 取服务器IP
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerIp(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$SERVERIP>", M2Share.Config.ServerIPaddr);
        }

        /// <summary>
        /// 取论坛地址
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBbsWeiSite(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BBSSITE>", M2Share.Config.sBbsSite);
        }

        /// <summary>
        /// 取客户端下载地址
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCilentDownLoad(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CLIENTDOWNLOAD>", M2Share.Config.sClientDownload);
        }

        /// <summary>
        /// 取掉落物品名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = PlayObject.ScatterItemName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMNAME>", sText);
        }

        /// <summary>
        /// 取物品掉落拥有者名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemownerName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = PlayObject.ScatterItemOwnerName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMOWNERNAME>", sText);
        }

        /// <summary>
        /// 取暴物品所在地图文件名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemMapName(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = PlayObject.ScatterItemMapName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMMAPNAME>", sText);
        }

        /// <summary>
        /// 取物品掉落地图名称
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemMapDesc(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = PlayObject.ScatterItemMapDesc;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMMAPDESC>", sText);
        }

        /// <summary>
        /// 取物品掉落所在地图X坐标
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemX(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.ScatterItemX).ToString();
            sMsg = CombineStr(sMsg, "<$SCATTERITEMX>", sText);
        }

        /// <summary>
        /// 取物品掉落所在地图Y坐标
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemY(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            string sText = (PlayObject.ScatterItemY).ToString();
            sMsg = CombineStr(sMsg, "<$SCATTERITEMX>", sText);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount0(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount0);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount1(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount1);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount2(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount2);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount3(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount3);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount4(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount4);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount5(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount5);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount6(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount6);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount7(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount7);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount8(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount8);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount9(PlayObject PlayObject, string sVariable, ref string sMsg)
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