using OpenMir2;
using OpenMir2.Data;
using OpenMir2.Packets.ClientPackets;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 删除指定玩家包裹物品
    /// </summary>
    [Command("DeleteItem", "删除人物身上指定的物品", help: "人物名称 物品名称 数量", 10)]
    public class DeleteItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            string sItemName = @params.Length > 1 ? @params[1] : ""; //物品名称
            int nCount = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0; //数量
            StdItem stdItem;
            UserItem userItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemCount = 0;
            for (int i = mIPlayerActor.ItemList.Count - 1; i >= 0; i--)
            {
                if (mIPlayerActor.ItemList.Count <= 0)
                {
                    break;
                }

                userItem = mIPlayerActor.ItemList[i];
                stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    mIPlayerActor.SendDelItems(userItem);
                    mIPlayerActor.ItemList.RemoveAt(i);
                    nItemCount++;
                    if (nItemCount >= nCount)
                    {
                        break;
                    }
                }
            }
        }
    }
}