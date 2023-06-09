using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 显示物品信息
    /// </summary>
    [Command("ShowUseItem", "显示物品信息", CommandHelp.GameCommandShowUseItemInfoHelpMsg, 10)]
    public class ShowUseItemInfoCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < mIPlayerActor.UseItems.Length; i++)
            {
                var userItem = mIPlayerActor.UseItems[i];
                if (userItem.Index == 0)
                {
                    continue;
                }
                PlayerActor.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]", ModuleShare.GetUseItemName(i), ItemSystem.GetStdItemName(userItem.Index), userItem.Index,
                    userItem.MakeIndex, userItem.Dura, userItem.DuraMax), MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}