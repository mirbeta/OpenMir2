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
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sMineName = @Params.Length > 0 ? @Params[0] : "";
            int nMineCount = @Params.Length > 0 ? HUtil32.StrToInt(@Params[1], 0) : 0;
            int nDura = @Params.Length > 0 ? HUtil32.StrToInt(@Params[2], 0) : 0;
            if (PlayObject.Permission < this.Command.PermissionMin) {
                PlayObject.SysMsg(CommandHelp.GameCommandPermissionTooLow, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sMineName) || !string.IsNullOrEmpty(sMineName) && sMineName[0] == '?' || nMineCount <= 0) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nDura <= 0) {
                nDura = M2Share.RandomNumber.Random(18) + 3;
            }
            // 如纯度不填,则随机给纯度
            for (int i = 0; i < nMineCount; i++) {
                UserItem UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sMineName, ref UserItem)) {
                    StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (StdItem != null && StdItem.StdMode == 43) {
                        if (PlayObject.IsAddWeightAvailable(StdItem.Weight * nMineCount)) {
                            UserItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (UserItem.Dura > UserItem.DuraMax) {
                                UserItem.Dura = UserItem.DuraMax;
                            }
                            PlayObject.ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            if (StdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(5, PlayObject.MapName + "\09" + PlayObject.CurrX + "\09" + PlayObject.CurrY + "\09" +
                                                                   PlayObject.ChrName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex + "\09" + UserItem.Dura + "/"
                                                                   + UserItem.DuraMax + "\09" + PlayObject.ChrName);
                            }
                        }
                    }
                }
                else {
                    UserItem = null;
                    break;
                }
            }
        }
    }
}