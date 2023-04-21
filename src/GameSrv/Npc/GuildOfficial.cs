using GameSrv.Castle;
using GameSrv.Player;
using GameSrv.Script;
using GameSrv.World;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc {
    /// <summary>
    /// 行会NPC类
    /// 行会管理NPC 如：比奇国王
    /// </summary>
    public class GuildOfficial : NormNpc {
        public GuildOfficial() : base() {
            RaceImg = ActorRace.Merchant;
            Appr = 8;
        }

        public override void Click(PlayObject PlayObject) {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(PlayObject PlayObject, string sVariable, ref string sMsg) {
            base.GetVariableText(PlayObject, sVariable, ref sMsg);
            if (sVariable == "$REQUESTCASTLELIST") {
                string sText = "";
                IList<string> List = new List<string>();
                GameShare.CastleMgr.GetCastleNameList(List);
                for (int i = 0; i < List.Count; i++) {
                    sText = sText + Format("<{0}/@requestcastlewarnow{1}> {2}", List[i], i, sText);
                }
                sText = sText + "\\ \\";
                sMsg = ReplaceVariableText(sMsg, "<$REQUESTCASTLELIST>", sText);
            }
        }

        public override void Run() {
            if (GameShare.RandomNumber.Random(40) == 0) {
                TurnTo(GameShare.RandomNumber.RandomByte(8));
            }
            else {
                if (GameShare.RandomNumber.Random(30) == 0) {
                    SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                }
            }
            base.Run();
        }

        public override void UserSelect(PlayObject PlayObject, string sData) {
            string sLabel = string.Empty;
            base.UserSelect(PlayObject, sData);
            try {
                if (!string.IsNullOrEmpty(sData) && sData.StartsWith("@")) {
                    string sMsg = HUtil32.GetValidStr3(sData, ref sLabel, '\r');
                    bool boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                    GotoLable(PlayObject, sLabel, !boCanJmp);
                    if (!boCanJmp) {
                        return;
                    }
                    if (string.Compare(sLabel, ScriptConst.sBUILDGUILDNOW, StringComparison.OrdinalIgnoreCase) == 0) {
                        ReQuestBuildGuild(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sSCL_GUILDWAR, StringComparison.OrdinalIgnoreCase) == 0) {
                        ReQuestGuildWar(PlayObject, sMsg);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sDONATE, StringComparison.OrdinalIgnoreCase) == 0) {
                        DoNate(PlayObject);
                    }
                    else if (HUtil32.CompareLStr(sLabel, ScriptConst.sREQUESTCASTLEWAR)) {
                        ReQuestCastleWar(PlayObject, sLabel[ScriptConst.sREQUESTCASTLEWAR.Length..]);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0) {
                        PlayObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0);
                    }
                    else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0) {
                        if (string.IsNullOrEmpty(PlayObject.ScriptGoBackLable)) {
                            PlayObject.ScriptGoBackLable = ScriptConst.sMAIN;
                        }
                        GotoLable(PlayObject, PlayObject.ScriptGoBackLable, false);
                    }
                }
            }
            catch (Exception ex) {
                GameShare.Logger.Error(ex);
            }
        }

        /// <summary>
        /// 请求建立行会
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sGuildName"></param>
        /// <returns></returns>
        private int ReQuestBuildGuild(PlayObject PlayObject, string sGuildName) {
            int result = 0;
            sGuildName = sGuildName.Trim();
            UserItem UserItem = null;
            if (string.IsNullOrEmpty(sGuildName)) {
                result = -4;
            }
            if (PlayObject.MyGuild == null) {
                if (PlayObject.Gold >= GameShare.Config.BuildGuildPrice) {
                    UserItem = PlayObject.CheckItems(GameShare.Config.WomaHorn);
                    if (UserItem == null) {
                        result = -3;// '你没有准备好需要的全部物品。'
                    }
                }
                else {
                    result = -2;// '缺少创建费用。'
                }
            }
            else {
                result = -1;// '您已经加入其它行会。'
            }
            if (result == 0) {
                if (GameShare.GuildMgr.AddGuild(sGuildName, PlayObject.ChrName)) {
                    WorldServer.SendServerGroupMsg(Messages.SS_205, GameShare.ServerIndex, sGuildName + '/' + PlayObject.ChrName);
                    PlayObject.SendDelItems(UserItem);
                    PlayObject.DelBagItem(UserItem.MakeIndex, GameShare.Config.WomaHorn);
                    PlayObject.DecGold(GameShare.Config.BuildGuildPrice);
                    PlayObject.GoldChanged();
                    PlayObject.MyGuild = GameShare.GuildMgr.MemberOfGuild(PlayObject.ChrName);
                    if (PlayObject.MyGuild != null) {
                        PlayObject.GuildRankName = PlayObject.MyGuild.GetRankName(PlayObject, ref PlayObject.GuildRankNo);
                        RefShowName();
                    }
                }
                else {
                    result = -4;
                }
            }
            if (result >= 0) {
                PlayObject.SendMsg(this, Messages.RM_BUILDGUILD_OK, 0, 0, 0, 0);
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_BUILDGUILD_FAIL, 0, result, 0, 0);
            }
            return result;
        }

        /// <summary>
        /// 请求行会战争
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sGuildName"></param>
        /// <returns></returns>
        private static void ReQuestGuildWar(PlayObject PlayObject, string sGuildName) {
            if (GameShare.GuildMgr.FindGuild(sGuildName) != null) {
                if (PlayObject.Gold >= GameShare.Config.GuildWarPrice) {
                    PlayObject.DecGold(GameShare.Config.GuildWarPrice);
                    PlayObject.GoldChanged();
                    PlayObject.ReQuestGuildWar(sGuildName);
                }
                else {
                    PlayObject.SysMsg("你没有足够的金币!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg("行会 " + sGuildName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        private void DoNate(PlayObject PlayObject) {
            PlayObject.SendMsg(this, Messages.RM_DONATE_OK, 0, 0, 0, 0);
        }

        private void ReQuestCastleWar(PlayObject PlayObject, string sIndex) {
            int nIndex = HUtil32.StrToInt(sIndex, -1);
            if (nIndex < 0) {
                nIndex = 0;
            }
            UserCastle Castle = GameShare.CastleMgr.GetCastle(nIndex);
            if (PlayObject.IsGuildMaster() && !Castle.IsMember(PlayObject)) {
                UserItem UserItem = PlayObject.CheckItems(GameShare.Config.ZumaPiece);
                if (UserItem != null) {
                    if (Castle.AddAttackerInfo(PlayObject.MyGuild)) {
                        PlayObject.SendDelItems(UserItem);
                        PlayObject.DelBagItem(UserItem.MakeIndex, GameShare.Config.ZumaPiece);
                        GotoLable(PlayObject, "~@request_ok", false);
                    }
                    else {
                        PlayObject.SysMsg("你现在无法请求攻城!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else {
                    PlayObject.SysMsg("你没有" + GameShare.Config.ZumaPiece + "!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg("你的请求被取消!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        protected override void SendCustemMsg(PlayObject PlayObject, string sMsg) {
            base.SendCustemMsg(PlayObject, sMsg);
        }
    }
}