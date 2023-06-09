using SystemModule;

namespace CommandModule.Commands
{
    [Command("MemberFunctionEx", "", help: "打开会员功能窗口")]
    public class MemberFunctionExCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (ModuleShare.FunctionNPC != null)
            {
                ModuleShare.FunctionNPC.GotoLable(PlayerActor, "@Member", false);
            }
        }
    }
}