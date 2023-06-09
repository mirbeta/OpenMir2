using M2Server.Castle;
using M2Server.Guild;
using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 行会传送，行会掌门人可以将整个行会成员全部集中。
    /// </summary>
    [Command("GuildRecall", "行会传送，行会掌门人可以将整个行会成员全部集中。")]
    public class GuildRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            if (!PlayerActor.SysMsgGuildMove && PlayerActor.Permission < 6) {
                PlayerActor.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!PlayerActor.IsGuildMaster()) {
                PlayerActor.SysMsg("行会掌门人才可以使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.Envir.Flag.NoGuildReCall) {
                PlayerActor.SysMsg("本地图不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var mCastle = SystemShare.CastleMgr.InCastleWarArea(IPlayerActor);
            if (mCastle != null && mCastle.UnderWar) {
                PlayerActor.SysMsg("攻城区域不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRecallCount = 0;
            var nNoRecallCount = 0;
            var dwValue = (HUtil32.GetTickCount() - PlayerActor.GroupRcallTick) / 1000;
            PlayerActor.GroupRcallTick = PlayerActor.GroupRcallTick + dwValue * 1000;
            if (PlayerActor.Permission >= 6) {
                PlayerActor.GroupRcallTime = 0;
            }
            if (PlayerActor.GroupRcallTime > dwValue) {
                PlayerActor.GroupRcallTime -= (short)dwValue;
            }
            else {
                PlayerActor.GroupRcallTime = 0;
            }
            if (PlayerActor.GroupRcallTime > 0) {
                PlayerActor.SysMsg($"{PlayerActor.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < PlayerActor.MyGuild.RankList.Count; i++) {
                var guildRank = PlayerActor.MyGuild.RankList[i];
                if (guildRank == null) {
                    continue;
                }
                for (var j = 0; j < guildRank.MemberList.Count; j++) {
                    var memberObject = SystemShare.WorldEngine.GetPlayObject(guildRank.MemberList[j].MemberName);
                    if (memberObject != null) {
                        if (memberObject == IPlayerActor) {
                            // Inc(nNoRecallCount);
                            continue;
                        }
                        if (memberObject.AllowGuildReCall) {
                            if (memberObject.Envir.Flag.NoReCall) {
                                PlayerActor.SysMsg($"{memberObject.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                            }
                            else {
                                PlayerActor.RecallHuman(memberObject.ChrName);
                                nRecallCount++;
                            }
                        }
                        else {
                            nNoRecallCount++;
                            PlayerActor.SysMsg($"{memberObject.ChrName} 不允许行会合一!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                }
            }
            PlayerActor.SysMsg($"已传送{nRecallCount}个成员，{nNoRecallCount}个成员未被传送。", MsgColor.Green, MsgType.Hint);
            PlayerActor.GroupRcallTick = HUtil32.GetTickCount();
            PlayerActor.GroupRcallTime = (short)SystemShare.Config.GuildRecallTime;
        }
    }
}