using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定物品名称
    /// </summary>
    [Command("ChangeItemName", "调整指定物品名称", 10, Help = "物品编号 物品ID号 物品名称")]
    public class ChangeItemNameCommand : GameCommand
    {
        [ExecuteCommand]
        public void ChangeItemName(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sMakeIndex = @params.Length > 0 ? @params[0] : "";
            string sItemIndex = @params.Length > 1 ? @params[1] : "";
            string sItemName = @params.Length > 2 ? @params[2] : "";
            if (sMakeIndex == "" || sItemIndex == "" || string.IsNullOrEmpty(sItemName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
            int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
            if (nMakeIndex <= 0 || nItemIndex < 0)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.CustomItemMgr.AddCustomItemName(nMakeIndex, nItemIndex, sItemName))
            {
                M2Share.CustomItemMgr.SaveCustomItemName();
                PlayObject.SysMsg("物品名称设置成功。", MsgColor.Green, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg("此物品，已经设置了其它的名称!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}