﻿using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : Commond
    {
        [ExecuteCommand]
        public void ReloadAbuse(PlayObject PlayObject)
        {

        }
    }
}