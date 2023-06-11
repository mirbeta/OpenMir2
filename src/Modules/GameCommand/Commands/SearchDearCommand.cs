using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 此命令用于查询配偶当前所在位置
    /// </summary>
    [Command("SearchDear", "此命令用于查询配偶当前所在位置")]
    public class SearchDearCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (string.IsNullOrEmpty(PlayerActor.DearName))
            {
                PlayerActor.SysMsg(Settings.YouAreNotMarryedMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.DearHuman == null)
            {
                if (PlayerActor.Gender == 0)
                {
                    PlayerActor.SysMsg(Settings.YourWifeNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(Settings.YourHusbandNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (PlayerActor.Gender == 0)
            {
                // '你的老婆现在位于:'
                PlayerActor.SysMsg(Settings.YourWifeNowLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.SysMsg(PlayerActor.DearHuman.ChrName + ' ' + PlayerActor.DearHuman.Envir.MapDesc + '(' +
                                  PlayerActor.DearHuman.CurrX + ':' + PlayerActor.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老公正在找你，他现在位于:'
                PlayerActor.DearHuman.SysMsg(Settings.YourHusbandSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.DearHuman.SysMsg(
                    PlayerActor.ChrName + ' ' + PlayerActor.Envir.MapDesc + '(' + PlayerActor.CurrX + ':' +
                    PlayerActor.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                // '你的老公现在位于:'
                PlayerActor.SysMsg(Settings.YourHusbandNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(PlayerActor.DearHuman.ChrName + ' ' + PlayerActor.DearHuman.Envir.MapDesc + '(' +
                                  PlayerActor.DearHuman.CurrX + ':' + PlayerActor.DearHuman.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老婆正在找你，她现在位于:'
                PlayerActor.DearHuman.SysMsg(Settings.YourWifeSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.DearHuman.SysMsg(PlayerActor.ChrName + ' ' + PlayerActor.Envir.MapDesc + '(' + PlayerActor.CurrX + ':' +
                                            PlayerActor.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
        }
    }
}