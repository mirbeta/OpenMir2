﻿using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令允许或禁止夫妻传送
    /// </summary>
    [Command("AllowDearRecall", "", 10)]
    public class AllowDearRecallCommand : Command
    {
        [ExecuteCommand]
        public void AllowDearRecall(string[] @Params, PlayObject PlayObject)
        {
            PlayObject.MBoCanDearRecall = !PlayObject.MBoCanDearRecall;
            if (PlayObject.MBoCanDearRecall)
            {
                PlayObject.SysMsg(CommandHelp.EnableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(CommandHelp.DisableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}