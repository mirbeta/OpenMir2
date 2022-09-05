using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Npc
{
    /// <summary>
    /// 行会NPC类
    /// 行会管理NPC 如：比奇国王
    /// </summary>
    public class TGuildOfficial : NormNpc
    {
        public TGuildOfficial() : base()
        {
            this.RaceImg = Grobal2.RCC_MERCHANT;
            this.Appr = 8;
        }

        public override void Click(PlayObject PlayObject)
        {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(PlayObject PlayObject, ref string sMsg, string sVariable)
        {
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            if (sVariable == "$REQUESTCASTLELIST")
            {
                var sText = "";
                IList<string> List = new List<string>();
                M2Share.CastleMgr.GetCastleNameList(List);
                for (var i = 0; i < List.Count; i++)
                {
                    sText = sText + format("<{0}/@requestcastlewarnow{1}> {2}", List[i], i.ToString(), sText);
                }
                sText = sText + "\\ \\";
                sMsg = this.ReplaceVariableText(sMsg, "<$REQUESTCASTLELIST>", sText);
            }
        }

        public override void Run()
        {
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                this.TurnTo(M2Share.RandomNumber.RandomByte(8));
            }
            else
            {
                if (M2Share.RandomNumber.Random(30) == 0)
                {
                    this.SendRefMsg(Grobal2.RM_HIT, this.Direction, this.CurrX, this.CurrY, 0, "");
                }
            }
            base.Run();
        }

        public override void UserSelect(PlayObject PlayObject, string sData)
        {
            var sMsg = string.Empty;
            var sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TGuildOfficial::UserSelect... ";
            base.UserSelect(PlayObject, sData);
            try
            {
                if (sData != "" && sData[0] == '@')
                {
                    sMsg = HUtil32.GetValidStr3(sData, ref sLabel, "\r");
                    var boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                    this.GotoLable(PlayObject, sLabel, !boCanJmp);
                    if (!boCanJmp)
                    {
                        return;
                    }
                    if (string.Compare(sLabel, ScriptConst.sBUILDGUILDNOW, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        ReQuestBuildGuild(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sSCL_GUILDWAR, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        ReQuestGuildWar(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sDONATE, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        DoNate(PlayObject);
                    }
                    else if (HUtil32.CompareLStr(sLabel, ScriptConst.sREQUESTCASTLEWAR, ScriptConst.sREQUESTCASTLEWAR.Length))
                    {
                        ReQuestCastleWar(PlayObject, sLabel.Substring(ScriptConst.sREQUESTCASTLEWAR.Length, sLabel.Length - ScriptConst.sREQUESTCASTLEWAR.Length));
                    }
                    else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                    }
                    else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (PlayObject.m_sScriptGoBackLable == "")
                        {
                            PlayObject.m_sScriptGoBackLable = ScriptConst.sMAIN;
                        }
                        this.GotoLable(PlayObject, PlayObject.m_sScriptGoBackLable, false);
                    }
                }
            }
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg);
            }
        }

        /// <summary>
        /// 请求建立行会
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sGuildName"></param>
        /// <returns></returns>
        private int ReQuestBuildGuild(PlayObject PlayObject, string sGuildName)
        {
            var result = 0;
            sGuildName = sGuildName.Trim();
            TUserItem UserItem = null;
            if (sGuildName == "")
            {
                result = -4;
            }
            if (PlayObject.MyGuild == null)
            {
                if (PlayObject.Gold >= M2Share.Config.nBuildGuildPrice)
                {
                    UserItem = PlayObject.CheckItems(M2Share.Config.sWomaHorn);
                    if (UserItem == null)
                    {
                        result = -3;// '你没有准备好需要的全部物品。'
                    }
                }
                else
                {
                    result = -2;// '缺少创建费用。'
                }
            }
            else
            {
                result = -1;// '您已经加入其它行会。'
            }
            if (result == 0)
            {
                if (M2Share.GuildMgr.AddGuild(sGuildName, PlayObject.CharName))
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_205, M2Share.ServerIndex, sGuildName + '/' + PlayObject.CharName);
                    PlayObject.SendDelItems(UserItem);
                    PlayObject.DelBagItem(UserItem.MakeIndex, M2Share.Config.sWomaHorn);
                    PlayObject.DecGold(M2Share.Config.nBuildGuildPrice);
                    PlayObject.GoldChanged();
                    PlayObject.MyGuild = M2Share.GuildMgr.MemberOfGuild(PlayObject.CharName);
                    if (PlayObject.MyGuild != null)
                    {
                        PlayObject.GuildRankName = PlayObject.MyGuild.GetRankName(PlayObject, ref PlayObject.GuildRankNo);
                        this.RefShowName();
                    }
                }
                else
                {
                    result = -4;
                }
            }
            if (result >= 0)
            {
                PlayObject.SendMsg(this, Grobal2.RM_BUILDGUILD_OK, 0, 0, 0, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_BUILDGUILD_FAIL, 0, result, 0, 0, "");
            }
            return result;
        }

        /// <summary>
        /// 请求行会战争
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sGuildName"></param>
        /// <returns></returns>
        private void ReQuestGuildWar(PlayObject PlayObject, string sGuildName)
        {
            if (M2Share.GuildMgr.FindGuild(sGuildName) != null)
            {
                if (PlayObject.Gold >= M2Share.Config.nGuildWarPrice)
                {
                    PlayObject.DecGold(M2Share.Config.nGuildWarPrice);
                    PlayObject.GoldChanged();
                    PlayObject.ReQuestGuildWar(sGuildName);
                }
                else
                {
                    PlayObject.SysMsg("你没有足够的金币!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("行会 " + sGuildName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        private void DoNate(PlayObject PlayObject)
        {
            PlayObject.SendMsg(this, Grobal2.RM_DONATE_OK, 0, 0, 0, 0, "");
        }

        private void ReQuestCastleWar(PlayObject PlayObject, string sIndex)
        {
            var nIndex = HUtil32.Str_ToInt(sIndex, -1);
            if (nIndex < 0)
            {
                nIndex = 0;
            }
            var Castle = M2Share.CastleMgr.GetCastle(nIndex);
            if (PlayObject.IsGuildMaster() && !Castle.IsMember(PlayObject))
            {
                var UserItem = PlayObject.CheckItems(M2Share.Config.sZumaPiece);
                if (UserItem != null)
                {
                    if (Castle.AddAttackerInfo(PlayObject.MyGuild))
                    {
                        PlayObject.SendDelItems(UserItem);
                        PlayObject.DelBagItem(UserItem.MakeIndex, M2Share.Config.sZumaPiece);
                        this.GotoLable(PlayObject, "~@request_ok", false);
                    }
                    else
                    {
                        PlayObject.SysMsg("你现在无法请求攻城!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("你没有" + M2Share.Config.sZumaPiece + "!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("你的请求被取消!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        protected override void SendCustemMsg(PlayObject PlayObject, string sMsg)
        {
            base.SendCustemMsg(PlayObject, sMsg);
        }
    }
}

