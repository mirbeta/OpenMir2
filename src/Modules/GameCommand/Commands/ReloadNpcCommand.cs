using SystemModule.Actors;

namespace CommandModule.Commands
{
    /// <summary>
    /// 重新加载当前12格范围内NPC
    /// </summary>
    [Command("ReloadNpc", "重新加载当前9格范围内NPC", 10)]
    public class ReloadNpcCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            string sParam = string.Empty;
            if (@params != null)
            {
                sParam = @params.Length > 0 ? @params[0] : "";
            }

            //Merchant merchant;
            //NormNpc npc;
            //if (string.Compare("all", sParam, StringComparison.CurrentCultureIgnoreCase) == 0)
            //{
            //    M2Share.LocalDb.ReLoadMerchants();
            //    SystemShare.WorldEngine.ReloadMerchantList();
            //    PlayerActor.SysMsg("交易NPC重新加载完成!!!", MsgColor.Red, MsgType.Hint);
            //    SystemShare.WorldEngine.ReloadNpcList();
            //    PlayerActor.SysMsg("管理NPC重新加载完成!!!", MsgColor.Red, MsgType.Hint);
            //    return;
            //}
            //else
            //{
            //    IList<BaseObject> tmpMerList = new List<BaseObject>();
            //    try
            //    {
            //        if (SystemShare.WorldEngine.GetMerchantList(PlayerActor.Envir, PlayerActor.CurrX, PlayerActor.CurrY, 9, tmpMerList) > 0)
            //        {
            //            for (var i = 0; i < tmpMerList.Count; i++)
            //            {
            //                merchant = (Merchant)tmpMerList[i];
            //                merchant.ClearScript();
            //                merchant.LoadMerchantScript();
            //                PlayerActor.SysMsg(merchant.ChrName + "重新加载成功...", MsgColor.Green, MsgType.Hint);
            //            }
            //        }
            //        else
            //        {
            //            PlayerActor.SysMsg("附近未发现任何交易NPC!!!", MsgColor.Red, MsgType.Hint);
            //        }

            //        IList<BaseObject> tmpNorList = new List<BaseObject>();
            //        if (SystemShare.WorldEngine.GetNpcList(PlayerActor.Envir, PlayerActor.CurrX, PlayerActor.CurrY, 9, tmpNorList) > 0)
            //        {
            //            for (var i = 0; i < tmpNorList.Count; i++)
            //            {
            //                npc = tmpNorList[i] as NormNpc;
            //                npc.ClearScript();
            //                npc.LoadNPCScript();
            //                PlayerActor.SysMsg(npc.ChrName + "重新加载成功...", MsgColor.Green, MsgType.Hint);
            //            }
            //        }
            //        else
            //        {
            //            PlayerActor.SysMsg("附近未发现任何管理NPC!!!", MsgColor.Red, MsgType.Hint);
            //        }
            //    }
            //    finally
            //    {
            //    }
            //}
        }
    }
}