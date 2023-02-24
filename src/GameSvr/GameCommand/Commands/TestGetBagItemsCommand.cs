using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    [Command("TestGetBagItems", "", 10)]
    public class TestGetBagItemsCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            byte btDc = 0;
            byte btSc = 0;
            byte btMc = 0;
            byte btDura = 0;
            //PlayObject.GetBagUseItems(ref btDc, ref btSc, ref btMc, ref btDura);
            PlayObject.SysMsg(string.Format("DC:%d SC:%d MC:%d DURA:%d", btDc, btSc, btMc, btDura), MsgColor.Blue, MsgType.Hint);
        }
    }
}