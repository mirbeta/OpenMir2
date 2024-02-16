using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
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
                PlayerActor.SysMsg(MessageSettings.YouAreNotMarryedMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.DearHuman == 0)
            {
                if (PlayerActor.Gender == 0)
                {
                    PlayerActor.SysMsg(MessageSettings.YourWifeNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(MessageSettings.YourHusbandNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            var playerDear = SystemShare.ActorMgr.Get<IPlayerActor>(PlayerActor.DearHuman);
            if (PlayerActor.Gender == 0)
            {
                // '你的老婆现在位于:'
                PlayerActor.SysMsg(MessageSettings.YourWifeNowLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayerActor.SysMsg(playerDear.ChrName + ' ' + playerDear.Envir.MapDesc + '(' + playerDear.CurrX + ':' + playerDear.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老公正在找你，他现在位于:'
                playerDear.SysMsg(MessageSettings.YourHusbandSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                playerDear.SysMsg(PlayerActor.ChrName + ' ' + PlayerActor.Envir.MapDesc + '(' + PlayerActor.CurrX + ':' + PlayerActor.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                // '你的老公现在位于:'
                PlayerActor.SysMsg(MessageSettings.YourHusbandNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(playerDear.ChrName + ' ' + playerDear.Envir.MapDesc + '(' + playerDear.CurrX + ':' + playerDear.CurrY + ')', MsgColor.Green, MsgType.Hint);

                // '你的老婆正在找你，她现在位于:'
                playerDear.SysMsg(MessageSettings.YourWifeSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                playerDear.SysMsg(PlayerActor.ChrName + ' ' + PlayerActor.Envir.MapDesc + '(' + PlayerActor.CurrX + ':' + PlayerActor.CurrY + ')', MsgColor.Green, MsgType.Hint);
            }
        }
    }
}