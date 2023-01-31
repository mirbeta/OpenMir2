using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [Command("SearchMaster", "查询师徒当前所在位置", 0)]
    public class SearchMasterCommand : Command
    {
        [ExecuteCommand]
        public static void SearchMaster(PlayObject PlayObject)
        {
            if (PlayObject.MasterName == "")
            {
                PlayObject.SysMsg(M2Share.g_sYouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.MBoMaster)
            {
                if (PlayObject.MasterList.Count <= 0)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (var i = 0; i < PlayObject.MasterList.Count; i++)
                {
                    var Human = PlayObject.MasterList[i];
                    PlayObject.SysMsg(Human.ChrName + " " + Human.Envir.MapDesc + "(" + Human.CurrX + ":" + Human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(M2Share.g_sYourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(PlayObject.ChrName + " " + PlayObject.Envir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                if (PlayObject.MasterHuman == null)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(PlayObject.MasterHuman.ChrName + " " + PlayObject.MasterHuman.Envir.MapDesc + "(" + PlayObject.MasterHuman.CurrX + ":"
                    + PlayObject.MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                PlayObject.MasterHuman.SysMsg(M2Share.g_sYourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.MasterHuman.SysMsg(PlayObject.ChrName + " " + PlayObject.Envir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}