using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands
{
    [Command("ClearBagItem", "清理包裹物品", "人物名称", 10)]
    public class ClearBagItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null)
            {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<DeleteItem> delList = null;
            if (mPlayObject.ItemList.Count > 0)
            {
                delList = new List<DeleteItem>();
                for (int i = mPlayObject.ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem userItem = mPlayObject.ItemList[i];
                    delList.Add(new DeleteItem()
                    {
                        ItemName = M2Share.WorldEngine.GetStdItemName(userItem.Index),
                        MakeIndex = userItem.MakeIndex
                    });
                    mPlayObject.ItemList.RemoveAt(i);
                }
                mPlayObject.ItemList.Clear();
            }
            if (delList != null)
            {
                int objectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(objectId, delList);
                mPlayObject.SendMsg(playObject, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
            }
        }
    }
}