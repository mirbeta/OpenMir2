using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace CommandSystem.Commands
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
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
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
            IList<DeleteItem> delList = null;
            if (mIPlayerActor.ItemList.Count > 0)
            {
                delList = new List<DeleteItem>();
                for (var i = mIPlayerActor.ItemList.Count - 1; i >= 0; i--)
                {
                    var userItem = mIPlayerActor.ItemList[i];
                    delList.Add(new DeleteItem()
                    {
                        ItemName = SystemShare.ItemSystem.GetStdItemName(userItem.Index),
                        MakeIndex = userItem.MakeIndex
                    });
                    mIPlayerActor.ItemList.RemoveAt(i);
                }
                mIPlayerActor.ItemList.Clear();
            }
            if (delList != null)
            {
                var objectId = HUtil32.Sequence();
                SystemShare.ActorMgr.AddOhter(objectId, delList);
                mIPlayerActor.SendMsg(PlayerActor, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
            }
        }
    }
}