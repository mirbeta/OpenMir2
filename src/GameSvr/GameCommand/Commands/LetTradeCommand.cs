﻿using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Lettrade", "", "", 0)]
    public class LetTradeCommand : GameCommand
    {
        [ExecuteCommand]
        public static void Lettrade(PlayObject playObject)
        {
            playObject.AllowDeal = !playObject.AllowDeal;
            if (playObject.AllowDeal)
            {
                playObject.SysMsg(CommandHelp.EnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}