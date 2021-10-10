using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("TestGetBagItems", "", 10)]
    public class TestGetBagItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void TestGetBagItems(TPlayObject PlayObject)
        {
            byte btDc = 0;
            byte btSc = 0;
            byte btMc = 0;
            byte btDura = 0;
            //PlayObject.GetBagUseItems(ref btDc, ref btSc, ref btMc, ref btDura);
            PlayObject.SysMsg(string.Format("DC:%d SC:%d MC:%d DURA:%d", btDc, btSc, btMc, btDura), TMsgColor.c_Blue, TMsgType.t_Hint);
        }
    }
}