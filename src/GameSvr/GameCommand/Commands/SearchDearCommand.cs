using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于查询配偶当前所在位置
    /// </summary>
    [Command("SearchDear", "此命令用于查询配偶当前所在位置", 0)]
    public class SearchDearCommand : Command
    {
        [ExecuteCommand]
        public static void SearchDear(PlayObject PlayObject)
        {
            if (PlayObject.DearName == "")
            {
                PlayObject.SysMsg(Settings.YouAreNotMarryedMsg, MsgColor.Red, MsgType.Hint);
                return;
            }

            if (PlayObject.DearHuman == null)
            {
                if (PlayObject.Gender == 0)
                {
                    PlayObject.SysMsg(Settings.YourWifeNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg(Settings.YourHusbandNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }

            if (PlayObject.Gender == 0)
            {
                // '你的老婆现在位于:'
                PlayObject.SysMsg(Settings.YourWifeNowLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.SysMsg(PlayObject.DearHuman.ChrName + ' ' + PlayObject.DearHuman.Envir.MapDesc + '(' + PlayObject.DearHuman.CurrX + ':' + PlayObject.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老公正在找你，他现在位于:'
                PlayObject.DearHuman.SysMsg(Settings.YourHusbandSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.DearHuman.SysMsg(PlayObject.ChrName + ' ' + PlayObject.Envir.MapDesc + '(' + PlayObject.CurrX + ':' + PlayObject.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                // '你的老公现在位于:'
                PlayObject.SysMsg(Settings.YourHusbandNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(PlayObject.DearHuman.ChrName + ' ' + PlayObject.DearHuman.Envir.MapDesc + '(' + PlayObject.DearHuman.CurrX + ':' + PlayObject.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老婆正在找你，她现在位于:'
                PlayObject.DearHuman.SysMsg(Settings.YourWifeSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.DearHuman.SysMsg(PlayObject.ChrName + ' ' + PlayObject.Envir.MapDesc + '(' + PlayObject.CurrX + ':' + PlayObject.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
        }
    }
}