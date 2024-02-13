using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
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
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
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
            for (int i = 0; i < mIPlayerActor.UseItems.Length; i++)
            {
                OpenMir2.Packets.ClientPackets.UserItem userItem = mIPlayerActor.UseItems[i];
                if (userItem.Index == 0)
                {
                    continue;
                }
                PlayerActor.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]", SystemShare.GetUseItemName(i), SystemShare.ItemSystem.GetStdItemName(userItem.Index), userItem.Index,
                    userItem.MakeIndex, userItem.Dura, userItem.DuraMax), MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}