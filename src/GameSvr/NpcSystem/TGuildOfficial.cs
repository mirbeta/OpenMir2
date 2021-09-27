using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 行会NPC类
    /// 行会管理NPC 如：比奇国王
    /// </summary>
    public class TGuildOfficial: TNormNpc
    {
        public TGuildOfficial() : base()
        {
            this.m_btRaceImg = Grobal2.RCC_MERCHANT;
            this.m_wAppr = 8;
        }
        
        public override void Click(TPlayObject PlayObject)
        {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(TPlayObject PlayObject, ref string sMsg, string sVariable)
        {
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            if (sVariable == "$REQUESTCASTLELIST")
            {
                var sText = "";
                IList<string> List = new List<string>();
                M2Share.CastleManager.GetCastleNameList(List);
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
                this.TurnTo((byte)M2Share.RandomNumber.Random(8));
            }
            else
            {
                if (M2Share.RandomNumber.Random(30) == 0)
                {
                    this.SendRefMsg(Grobal2.RM_HIT, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
                }
            }
            base.Run();
        }

        public override void UserSelect(TPlayObject PlayObject, string sData)
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
                    if (string.Compare(sLabel, M2Share.sBUILDGUILDNOW, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        ReQuestBuildGuild(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, M2Share.sSCL_GUILDWAR, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        ReQuestGuildWar(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, M2Share.sDONATE, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        DoNate(PlayObject);
                    }
                    else if (HUtil32.CompareLStr(sLabel, M2Share.sREQUESTCASTLEWAR, M2Share.sREQUESTCASTLEWAR.Length))
                    {
                        ReQuestCastleWar(PlayObject, sLabel.Substring(M2Share.sREQUESTCASTLEWAR.Length, sLabel.Length - M2Share.sREQUESTCASTLEWAR.Length));
                    }
                    else if (string.Compare(sLabel, M2Share.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                    }
                    else if (string.Compare(sLabel, M2Share.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (PlayObject.m_sScriptGoBackLable == "")
                        {
                            PlayObject.m_sScriptGoBackLable = M2Share.sMAIN;
                        }
                        this.GotoLable(PlayObject, PlayObject.m_sScriptGoBackLable, false);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        private int ReQuestBuildGuild(TPlayObject PlayObject, string sGuildName)
        {
            var result = 0;
            sGuildName = sGuildName.Trim();
            TUserItem UserItem = null;
            if (sGuildName == "")
            {
                result =  -4;
            }
            if (PlayObject.m_MyGuild == null)
            {
                if (PlayObject.m_nGold >= M2Share.g_Config.nBuildGuildPrice)
                {
                    UserItem = PlayObject.CheckItems(M2Share.g_Config.sWomaHorn);
                    if (UserItem == null)
                    {
                        result =  -3;// '你没有准备好需要的全部物品。'
                    }
                }
                else
                {
                    result =  -2;// '缺少创建费用。'
                }
            }
            else
            {
                result =  -1;// '您已经加入其它行会。'
            }
            if (result == 0)
            {
                if (M2Share.GuildManager.AddGuild(sGuildName, PlayObject.m_sCharName))
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_205, M2Share.nServerIndex, sGuildName + '/' + PlayObject.m_sCharName);
                    PlayObject.SendDelItems(UserItem);
                    PlayObject.DelBagItem(UserItem.MakeIndex, M2Share.g_Config.sWomaHorn);
                    PlayObject.DecGold(M2Share.g_Config.nBuildGuildPrice);
                    PlayObject.GoldChanged();
                    PlayObject.m_MyGuild = M2Share.GuildManager.MemberOfGuild(PlayObject.m_sCharName);
                    if (PlayObject.m_MyGuild != null)
                    {
                        PlayObject.m_sGuildRankName = PlayObject.m_MyGuild.GetRankName(PlayObject, ref PlayObject.m_nGuildRankNo);
                        this.RefShowName();
                    }
                }
                else
                {
                    result =  -4;
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

        private int ReQuestGuildWar(TPlayObject PlayObject, string sGuildName)
        {
            int result;
            if (M2Share.GuildManager.FindGuild(sGuildName) != null)
            {
                if (PlayObject.m_nGold >= M2Share.g_Config.nGuildWarPrice)
                {
                    PlayObject.DecGold(M2Share.g_Config.nGuildWarPrice);
                    PlayObject.GoldChanged();
                    PlayObject.ReQuestGuildWar(sGuildName);
                }
                else
                {
                    PlayObject.SysMsg("你没有足够的金币!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("行会 " + sGuildName + " 不存在!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            result = 1;
            return result;
        }

        private void DoNate(TPlayObject PlayObject)
        {
            PlayObject.SendMsg(this, Grobal2.RM_DONATE_OK, 0, 0, 0, 0, "");
        }

        private void ReQuestCastleWar(TPlayObject PlayObject, string sIndex)
        {
            var nIndex = HUtil32.Str_ToInt(sIndex, -1);
            if (nIndex < 0)
            {
                nIndex = 0;
            }
            var Castle = M2Share.CastleManager.GetCastle(nIndex);
            if (PlayObject.IsGuildMaster() && !Castle.IsMember(PlayObject))
            {
                var UserItem = PlayObject.CheckItems(M2Share.g_Config.sZumaPiece);
                if (UserItem != null)
                {
                    if (Castle.AddAttackerInfo(PlayObject.m_MyGuild))
                    {
                        PlayObject.SendDelItems(UserItem);
                        PlayObject.DelBagItem(UserItem.MakeIndex, M2Share.g_Config.sZumaPiece);
                        this.GotoLable(PlayObject, "~@request_ok", false);
                    }
                    else
                    {
                        PlayObject.SysMsg("你现在无法请求攻城!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("你没有" + M2Share.g_Config.sZumaPiece + "!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("你的请求被取消!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        protected override void SendCustemMsg(TPlayObject PlayObject, string sMsg)
        {
            base.SendCustemMsg(PlayObject, sMsg);
        }
    }
}

