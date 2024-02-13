using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 将指定玩家添加到禁止人物列表
    /// </summary>
    [Command("DenyChrNameLogon", "将指定玩家添加到禁止人物列表", "人物名称 是否永久封(0,1)", 10)]
    public class DenyChrNameLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sChrName = @params.Length > 0 ? @params[0] : "";
            var sFixDeny = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sChrName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1')
                {
                    //Settings.g_DenyChrNameList.Add(sChrName, ((1) as Object));
                    SystemShare.SaveDenyChrNameList();
                    PlayerActor.SysMsg(sChrName + "已加入禁止人物列表", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    //Settings.g_DenyChrNameList.Add(sChrName, ((0) as Object));
                    PlayerActor.SysMsg(sChrName + "已加入临时禁止人物列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally
            {
            }
        }
    }
}