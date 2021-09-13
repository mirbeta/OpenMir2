using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [GameCommand("RecallHuman", "将指定人物召唤到身边(支持权限分配)", 10)]
    public class RecallHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void RecallHuman(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandRecallHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.RecallHuman(sHumanName);
        }
    }
}