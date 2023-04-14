using GameSrv.Items;
using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 给指定纯度的矿石
    /// </summary>
    [Command("GiveMine", "给指定纯度的矿石", "矿石名称 数量 持久", 10)]
    public class GiveMineCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sMineName = @params.Length > 0 ? @params[0] : "";
            var nMineCount = @params.Length > 0 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nDura = @params.Length > 0 ? HUtil32.StrToInt(@params[2], 0) : 0;
            if (playObject.Permission < this.Command.PermissionMin) {
                playObject.SysMsg(CommandHelp.GameCommandPermissionTooLow, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sMineName) || !string.IsNullOrEmpty(sMineName) && sMineName[0] == '?' || nMineCount <= 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nDura <= 0) {
                nDura = M2Share.RandomNumber.Random(18) + 3;
            }
            // 如纯度不填,则随机给纯度
            for (var i = 0; i < nMineCount; i++) {
                var userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sMineName, ref userItem)) {
                    var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem != null && stdItem.StdMode == 43) {
                        if (playObject.IsAddWeightAvailable(stdItem.Weight * nMineCount)) {
                            userItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (userItem.Dura > userItem.DuraMax) {
                                userItem.Dura = userItem.DuraMax;
                            }
                            playObject.ItemList.Add(userItem);
                            playObject.SendAddItem(userItem);
                            if (stdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(5, playObject.MapName + "\09" + playObject.CurrX + "\09" + playObject.CurrY + "\09" +
                                                                   playObject.ChrName + "\09" + stdItem.Name + "\09" + userItem.MakeIndex + "\09" + userItem.Dura + "/"
                                                                   + userItem.DuraMax + "\09" + playObject.ChrName);
                            }
                        }
                    }
                }
                else {
                    userItem = null;
                    break;
                }
            }
        }
    }
}