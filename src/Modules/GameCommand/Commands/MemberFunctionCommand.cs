using SystemModule;

namespace CommandSystem.Commands
{
    [Command("MemberFunction", "", help: "打开会员功能窗口")]
    public class MemberFunctionCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (SystemShare.ManageNPC != null)
            {
                PlayerActor.ScriptGotoCount = 0;
                SystemShare.ManageNPC.GotoLable(PlayerActor, "@Member", false);
            }
        }
    }
}