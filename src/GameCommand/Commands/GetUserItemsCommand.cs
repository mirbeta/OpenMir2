using M2Server.Items;
using M2Server.Player;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 取指定玩家物品
    /// </summary>
    [Command("GetUserItems", "取指定玩家物品", "人物名称 物品名称 数量 类型(0,1,2)", 10)]
    public class GetUserItemsCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sItemName = @params.Length > 1 ? @params[1] : "";
            var sItemCount = @params.Length > 2 ? @params[2] : "";
            var sType = @params.Length > 3 ? @params[3] : "";

            int nItemCount;
            StdItem stdItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName) || string.IsNullOrEmpty(sItemCount) || string.IsNullOrEmpty(sType)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nCount = HUtil32.StrToInt(sItemCount, 0);
            var nType = HUtil32.StrToInt(sType, 0);
            UserItem userItem;
            switch (nType) {
                case 0:
                    nItemCount = 0;
                    for (var i = 0; i < mPlayObject.UseItems.Length; i++) {
                        if (mPlayObject.ItemList.Count >= 46) {
                            break;
                        }
                        userItem = mPlayObject.UseItems[i];
                        stdItem = ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (!mPlayObject.IsEnoughBag()) {
                                break;
                            }
                            userItem = new UserItem(); ;
                            userItem = mPlayObject.UseItems[i];
                            mPlayObject.ItemList.Add(userItem);
                            mPlayObject.SendAddItem(userItem);
                            mPlayObject.UseItems[i].Index = 0;
                            nItemCount++;
                            if (nItemCount >= nCount) {
                                break;
                            }
                        }
                    }
                    mPlayObject.SendUseItems();
                    break;

                case 1:
                    nItemCount = 0;
                    for (var i = mPlayObject.ItemList.Count - 1; i >= 0; i--) {
                        if (mPlayObject.ItemList.Count >= 46) {
                            break;
                        }
                        if (mPlayObject.ItemList.Count <= 0) {
                            break;
                        }
                        userItem = mPlayObject.ItemList[i];
                        stdItem = ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (!mPlayObject.IsEnoughBag()) {
                                break;
                            }
                            mPlayObject.SendDelItems(userItem);
                            mPlayObject.ItemList.RemoveAt(i);
                            mPlayObject.ItemList.Add(userItem);
                            mPlayObject.SendAddItem(userItem);
                            nItemCount++;
                            if (nItemCount >= nCount) {
                                break;
                            }
                        }
                    }
                    break;

                case 2:
                    nItemCount = 0;
                    for (var i = mPlayObject.StorageItemList.Count - 1; i >= 0; i--) {
                        if (mPlayObject.ItemList.Count >= 46) {
                            break;
                        }
                        if (mPlayObject.StorageItemList.Count <= 0) {
                            break;
                        }
                        userItem = mPlayObject.StorageItemList[i];
                        stdItem = ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (!mPlayObject.IsEnoughBag()) {
                                break;
                            }
                            mPlayObject.StorageItemList.RemoveAt(i);
                            mPlayObject.ItemList.Add(userItem);
                            mPlayObject.SendAddItem(userItem);
                            nItemCount++;
                            if (nItemCount >= nCount) {
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}