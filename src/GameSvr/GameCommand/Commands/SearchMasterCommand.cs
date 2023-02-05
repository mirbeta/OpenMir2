using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [Command("SearchMaster", "查询师徒当前所在位置", 0)]
    public class SearchMasterCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject)
        {
            if (PlayObject.MasterName == "")
            {
                PlayObject.SysMsg(Settings.YouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.IsMaster)
            {
                if (PlayObject.MasterList.Count <= 0)
                {
                    PlayObject.SysMsg(Settings.YourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(Settings.YourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (int i = 0; i < PlayObject.MasterList.Count; i++)
                {
                    PlayObject Human = PlayObject.MasterList[i];
                    PlayObject.SysMsg(Human.ChrName + " " + Human.Envir.MapDesc + "(" + Human.CurrX + ":" + Human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(Settings.YourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(PlayObject.ChrName + " " + PlayObject.Envir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                if (PlayObject.MasterHuman == null)
                {
                    PlayObject.SysMsg(Settings.YourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(Settings.YourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(PlayObject.MasterHuman.ChrName + " " + PlayObject.MasterHuman.Envir.MapDesc + "(" + PlayObject.MasterHuman.CurrX + ":"
                    + PlayObject.MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                PlayObject.MasterHuman.SysMsg(Settings.YourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.MasterHuman.SysMsg(PlayObject.ChrName + " " + PlayObject.Envir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}