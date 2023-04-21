using GameSrv.Player;
using M2Server;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于查询配偶当前所在位置
    /// </summary>
    [Command("SearchDear", "此命令用于查询配偶当前所在位置")]
    public class SearchDearCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject playObject)
        {
            if (string.IsNullOrEmpty(playObject.DearName))
            {
                playObject.SysMsg(Settings.YouAreNotMarryedMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.DearHuman == null)
            {
                if (playObject.Gender == 0)
                {
                    playObject.SysMsg(Settings.YourWifeNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(Settings.YourHusbandNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (playObject.Gender == 0)
            {
                // '你的老婆现在位于:'
                playObject.SysMsg(Settings.YourWifeNowLocateMsg, MsgColor.Green, MsgType.Hint);
                playObject.SysMsg(playObject.DearHuman.ChrName + ' ' + playObject.DearHuman.Envir.MapDesc + '(' +
                                  playObject.DearHuman.CurrX + ':' + playObject.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老公正在找你，他现在位于:'
                playObject.DearHuman.SysMsg(Settings.YourHusbandSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                playObject.DearHuman.SysMsg(
                    playObject.ChrName + ' ' + playObject.Envir.MapDesc + '(' + playObject.CurrX + ':' +
                    playObject.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                // '你的老公现在位于:'
                playObject.SysMsg(Settings.YourHusbandNowLocateMsg, MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(playObject.DearHuman.ChrName + ' ' + playObject.DearHuman.Envir.MapDesc + '(' +
                                  playObject.DearHuman.CurrX + ':' + playObject.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老婆正在找你，她现在位于:'
                playObject.DearHuman.SysMsg(Settings.YourWifeSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                playObject.DearHuman.SysMsg(playObject.ChrName + ' ' + playObject.Envir.MapDesc + '(' + playObject.CurrX + ':' +
                                            playObject.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
        }
    }
}