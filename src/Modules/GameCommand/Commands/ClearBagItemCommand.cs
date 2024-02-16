using OpenMir2;
using OpenMir2.Data;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ClearBagItem", "清理包裹物品", "人物名称", 10)]
    public class ClearBagItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
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
            IList<DeleteItem> delList = null;
            if (mIPlayerActor.ItemList.Count > 0)
            {
                delList = new List<DeleteItem>();
                for (int i = mIPlayerActor.ItemList.Count - 1; i >= 0; i--)
                {
                    OpenMir2.Packets.ClientPackets.UserItem userItem = mIPlayerActor.ItemList[i];
                    delList.Add(new DeleteItem()
                    {
                        ItemName = SystemShare.EquipmentSystem.GetStdItemName(userItem.Index),
                        MakeIndex = userItem.MakeIndex
                    });
                    mIPlayerActor.ItemList.RemoveAt(i);
                }
                mIPlayerActor.ItemList.Clear();
            }
            if (delList != null)
            {
                int objectId = HUtil32.Sequence();
                SystemShare.ActorMgr.AddOhter(objectId, delList);
                mIPlayerActor.SendMsg(PlayerActor, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
            }
        }
    }
}