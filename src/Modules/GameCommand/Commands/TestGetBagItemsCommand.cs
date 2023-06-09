using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("TestGetBagItems", "", 10)]
    public class TestGetBagItemsCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            byte btDc = 0;
            byte btSc = 0;
            byte btMc = 0;
            byte btDura = 0;
            //PlayerActor.SysMsgGetBagUseItems(ref btDc, ref btSc, ref btMc, ref btDura);
            PlayerActor.SysMsg(string.Format("DC:%d SC:%d MC:%d DURA:%d", btDc, btSc, btMc, btDura), MsgColor.Blue, MsgType.Hint);
        }
    }
}