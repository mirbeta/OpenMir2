using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    [Command("ClearBagItem", "清理包裹物品", "人物名称", 10)]
    public class ClearBagItemCommand : Commond
    {
        [ExecuteCommand]
        public void ClearBagItem(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<DeleteItem> DelList = null;
            if (m_PlayObject.ItemList.Count > 0)
            {
                for (var i = m_PlayObject.ItemList.Count - 1; i >= 0; i--)
                {
                    var UserItem = m_PlayObject.ItemList[i];
                    if (DelList == null)
                    {
                        DelList = new List<DeleteItem>();
                    }
                    DelList.Add(new DeleteItem()
                    {
                        ItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index),
                        MakeIndex = UserItem.MakeIndex
                    });
                    m_PlayObject.ItemList.RemoveAt(i);
                }
                m_PlayObject.ItemList.Clear();
            }
            if (DelList != null)
            {
                var ObjectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(ObjectId, DelList);
                m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
            }
        }
    }
}