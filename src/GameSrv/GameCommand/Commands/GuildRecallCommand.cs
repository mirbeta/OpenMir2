using GameSrv.Castle;
using GameSrv.Guild;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 行会传送，行会掌门人可以将整个行会成员全部集中。
    /// </summary>
    [Command("GuildRecall", "行会传送，行会掌门人可以将整个行会成员全部集中。")]
    public class GuildRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (!playObject.GuildMove && playObject.Permission < 6) {
                playObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!playObject.IsGuildMaster()) {
                playObject.SysMsg("行会掌门人才可以使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.Envir.Flag.NoGuildReCall) {
                playObject.SysMsg("本地图不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var mCastle = M2Share.CastleMgr.InCastleWarArea(playObject);
            if (mCastle != null && mCastle.UnderWar) {
                playObject.SysMsg("攻城区域不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRecallCount = 0;
            var nNoRecallCount = 0;
            var dwValue = (HUtil32.GetTickCount() - playObject.GroupRcallTick) / 1000;
            playObject.GroupRcallTick = playObject.GroupRcallTick + dwValue * 1000;
            if (playObject.Permission >= 6) {
                playObject.GroupRcallTime = 0;
            }
            if (playObject.GroupRcallTime > dwValue) {
                playObject.GroupRcallTime -= (short)dwValue;
            }
            else {
                playObject.GroupRcallTime = 0;
            }
            if (playObject.GroupRcallTime > 0) {
                playObject.SysMsg($"{playObject.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < playObject.MyGuild.RankList.Count; i++) {
                var guildRank = playObject.MyGuild.RankList[i];
                if (guildRank == null) {
                    continue;
                }
                for (var j = 0; j < guildRank.MemberList.Count; j++) {
                    var memberObject = M2Share.WorldEngine.GetPlayObject(guildRank.MemberList[j].MemberName);
                    if (memberObject != null) {
                        if (memberObject == playObject) {
                            // Inc(nNoRecallCount);
                            continue;
                        }
                        if (memberObject.AllowGuildReCall) {
                            if (memberObject.Envir.Flag.NoReCall) {
                                playObject.SysMsg($"{memberObject.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                            }
                            else {
                                playObject.RecallHuman(memberObject.ChrName);
                                nRecallCount++;
                            }
                        }
                        else {
                            nNoRecallCount++;
                            playObject.SysMsg($"{memberObject.ChrName} 不允许行会合一!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                }
            }
            playObject.SysMsg($"已传送{nRecallCount}个成员，{nNoRecallCount}个成员未被传送。", MsgColor.Green, MsgType.Hint);
            playObject.GroupRcallTick = HUtil32.GetTickCount();
            playObject.GroupRcallTime = (short)M2Share.Config.GuildRecallTime;
        }
    }
}