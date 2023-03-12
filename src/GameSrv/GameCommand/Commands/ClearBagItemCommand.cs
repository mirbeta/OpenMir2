using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    [Command("ClearBagItem", "清理包裹物品", "人物名称", 10)]
    public class ClearBagItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<DeleteItem> DelList = null;
            if (m_PlayObject.ItemList.Count > 0) {
                DelList = new List<DeleteItem>();
                for (int i = m_PlayObject.ItemList.Count - 1; i >= 0; i--) {
                    UserItem UserItem = m_PlayObject.ItemList[i];
                    DelList.Add(new DeleteItem() {
                        ItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index),
                        MakeIndex = UserItem.MakeIndex
                    });
                    m_PlayObject.ItemList.RemoveAt(i);
                }
                m_PlayObject.ItemList.Clear();
            }
            if (DelList != null) {
                int ObjectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(ObjectId, DelList);
                m_PlayObject.SendMsg(m_PlayObject, Messages.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
            }
        }
    }
}