using OpenMir2;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定物品名称
    /// </summary>
    [Command("ChangeItemName", "调整指定物品名称", 10, Help = "物品编号 物品ID号 物品名称")]
    public class ChangeItemNameCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sMakeIndex = @params.Length > 0 ? @params[0] : "";
            string sItemIndex = @params.Length > 1 ? @params[1] : "";
            string sItemName = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sMakeIndex) || string.IsNullOrEmpty(sItemIndex) || string.IsNullOrEmpty(sItemName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
            int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
            if (nMakeIndex <= 0 || nItemIndex < 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            //if (ModuleShare.CustomItemMgr.AddCustomItemName(nMakeIndex, nItemIndex, sItemName))
            //{
            //    ModuleShare.CustomItemMgr.SaveCustomItemName();
            //    PlayerActor.SysMsg("物品名称设置成功。", MsgColor.Green, MsgType.Hint);
            //    return;
            //}
            PlayerActor.SysMsg("此物品，已经设置了其它的名称!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}