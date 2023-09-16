using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [Command("SearchMaster", "查询师徒当前所在位置")]
    public class SearchMasterCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (string.IsNullOrEmpty(PlayerActor.MasterName))
            {
                PlayerActor.SysMsg(MessageSettings.YouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.IsMaster)
            {
                if (PlayerActor.MasterList.Count <= 0)
                {
                    PlayerActor.SysMsg(MessageSettings.YourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayerActor.SysMsg(MessageSettings.YourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (var i = 0; i < PlayerActor.MasterList.Count; i++)
                {
                    var human = (IPlayerActor)PlayerActor.MasterList[i];
                    PlayerActor.SysMsg(human.ChrName + " " + human.Envir.MapDesc + "(" + human.CurrX + ":" + human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    human.SysMsg(MessageSettings.YourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    human.SysMsg(PlayerActor.ChrName + " " + PlayerActor.Envir.MapDesc + "(" + PlayerActor.CurrX + ":" + PlayerActor.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                if (PlayerActor.MasterHuman == null)
                {
                    PlayerActor.SysMsg(MessageSettings.YourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayerActor.SysMsg(MessageSettings.YourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(PlayerActor.MasterHuman.ChrName + " " + PlayerActor.MasterHuman.Envir.MapDesc + "(" + PlayerActor.MasterHuman.CurrX + ":"
                    + PlayerActor.MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                PlayerActor.MasterHuman.SysMsg(MessageSettings.YourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.MasterHuman.SysMsg(PlayerActor.ChrName + " " + PlayerActor.Envir.MapDesc + "(" + PlayerActor.CurrX + ":" + PlayerActor.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}