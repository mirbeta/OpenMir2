using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
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
                PlayerActor.SysMsg(Settings.YouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.IsMaster)
            {
                if (PlayerActor.MasterList.Count <= 0)
                {
                    PlayerActor.SysMsg(Settings.YourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayerActor.SysMsg(Settings.YourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (var i = 0; i < PlayerActor.MasterList.Count; i++)
                {
                    var human = (IPlayerActor)PlayerActor.MasterList[i];
                    PlayerActor.SysMsg(human.ChrName + " " + human.Envir.MapDesc + "(" + human.CurrX + ":" + human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    human.SysMsg(Settings.YourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    human.SysMsg(PlayerActor.ChrName + " " + PlayerActor.Envir.MapDesc + "(" + PlayerActor.CurrX + ":" + PlayerActor.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                if (PlayerActor.MasterHuman == null)
                {
                    PlayerActor.SysMsg(Settings.YourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayerActor.SysMsg(Settings.YourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(PlayerActor.MasterHuman.ChrName + " " + PlayerActor.MasterHuman.Envir.MapDesc + "(" + PlayerActor.MasterHuman.CurrX + ":"
                    + PlayerActor.MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                PlayerActor.MasterHuman.SysMsg(Settings.YourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.MasterHuman.SysMsg(PlayerActor.ChrName + " " + PlayerActor.Envir.MapDesc + "(" + PlayerActor.CurrX + ":" + PlayerActor.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}