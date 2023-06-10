using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace CommandModule.Commands
{
    /// <summary>
    /// 取指定玩家物品
    /// </summary>
    [Command("GetUserItems", "取指定玩家物品", "人物名称 物品名称 数量 类型(0,1,2)", 10)]
    public class GetUserItemsCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sItemName = @params.Length > 1 ? @params[1] : "";
            var sItemCount = @params.Length > 2 ? @params[2] : "";
            var sType = @params.Length > 3 ? @params[3] : "";

            int nItemCount;
            StdItem stdItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName) || string.IsNullOrEmpty(sItemCount) || string.IsNullOrEmpty(sType))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nCount = HUtil32.StrToInt(sItemCount, 0);
            var nType = HUtil32.StrToInt(sType, 0);
            UserItem userItem;
            switch (nType)
            {
                case 0:
                    nItemCount = 0;
                    for (var i = 0; i < mIPlayerActor.UseItems.Length; i++)
                    {
                        if (mIPlayerActor.ItemList.Count >= 46)
                        {
                            break;
                        }
                        userItem = mIPlayerActor.UseItems[i];
                        stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!mIPlayerActor.IsEnoughBag)
                            {
                                break;
                            }
                            userItem = new UserItem(); ;
                            userItem = mIPlayerActor.UseItems[i];
                            mIPlayerActor.ItemList.Add(userItem);
                            mIPlayerActor.SendAddItem(userItem);
                            mIPlayerActor.UseItems[i].Index = 0;
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    mIPlayerActor.SendUseItems();
                    break;

                case 1:
                    nItemCount = 0;
                    for (var i = mIPlayerActor.ItemList.Count - 1; i >= 0; i--)
                    {
                        if (mIPlayerActor.ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (mIPlayerActor.ItemList.Count <= 0)
                        {
                            break;
                        }
                        userItem = mIPlayerActor.ItemList[i];
                        stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!mIPlayerActor.IsEnoughBag)
                            {
                                break;
                            }
                            mIPlayerActor.SendDelItems(userItem);
                            mIPlayerActor.ItemList.RemoveAt(i);
                            mIPlayerActor.ItemList.Add(userItem);
                            mIPlayerActor.SendAddItem(userItem);
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    break;

                case 2:
                    nItemCount = 0;
                    for (var i = mIPlayerActor.StorageItemList.Count - 1; i >= 0; i--)
                    {
                        if (mIPlayerActor.ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (mIPlayerActor.StorageItemList.Count <= 0)
                        {
                            break;
                        }
                        userItem = mIPlayerActor.StorageItemList[i];
                        stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!mIPlayerActor.IsEnoughBag)
                            {
                                break;
                            }
                            mIPlayerActor.StorageItemList.RemoveAt(i);
                            mIPlayerActor.ItemList.Add(userItem);
                            mIPlayerActor.SendAddItem(userItem);
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}