using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("TestGetBagItems", "", 10)]
    public class TestGetBagItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void TestGetBagItems(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = @Params.Length > 0 ? @Params[0] : "";
            if ((sParam != "") && (sParam[1] == '?'))
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandTestGetBagItemsHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            byte btDc = 0;
            byte btSc = 0;
            byte btMc = 0;
            byte btDura = 0;
            //PlayObject.GetBagUseItems(ref btDc, ref btSc, ref btMc, ref btDura);
            PlayObject.SysMsg(string.Format("DC:%d SC:%d MC:%d DURA:%d", btDc, btSc, btMc, btDura), TMsgColor.c_Blue, TMsgType.t_Hint);
        }
    }
}