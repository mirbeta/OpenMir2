using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadAbuse(TPlayObject PlayObject)
        {
 
        }
    }
}