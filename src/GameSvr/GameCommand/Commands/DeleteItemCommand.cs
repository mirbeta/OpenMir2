using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 删除指定玩家包裹物品
    /// </summary>
    [Command("DeleteItem", "删除人物身上指定的物品", help: "人物名称 物品名称 数量", 10)]
    public class DeleteItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void DeleteItem(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : ""; //玩家名称
            string sItemName = @Params.Length > 1 ? @Params[1] : ""; //物品名称
            int nCount = @Params.Length > 2 ? int.Parse(@Params[2]) : 0; //数量
            Items.StdItem StdItem;
            UserItem UserItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemCount = 0;
            for (int i = m_PlayObject.ItemList.Count - 1; i >= 0; i--)
            {
                if (m_PlayObject.ItemList.Count <= 0)
                {
                    break;
                }

                UserItem = m_PlayObject.ItemList[i];
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.SendDelItems(UserItem);
                    m_PlayObject.ItemList.RemoveAt(i);
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