using GameSvr.Actor;
using GameSvr.Npc;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 重新加载当前12格范围内NPC
    /// </summary>
    [Command("ReloadNpc", "重新加载当前9格范围内NPC", 10)]
    public class ReloadNpcCommand : Command
    {
        [ExecuteCommand]
        public static void ReloadNpc(string[] @Params, PlayObject PlayObject)
        {
            var sParam = string.Empty;
            if (@Params != null)
            {
                sParam = @Params.Length > 0 ? @Params[0] : "";
            }

            Merchant Merchant;
            NormNpc NPC;
            if (string.Compare("all", sParam, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                M2Share.LocalDb.ReLoadMerchants();
                M2Share.WorldEngine.ReloadMerchantList();
                PlayObject.SysMsg("交易NPC重新加载完成!!!", MsgColor.Red, MsgType.Hint);
                M2Share.WorldEngine.ReloadNpcList();
                PlayObject.SysMsg("管理NPC重新加载完成!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            else
            {
                IList<BaseObject> TmpMerList = new List<BaseObject>();
                try
                {
                    if (M2Share.WorldEngine.GetMerchantList(PlayObject.Envir, PlayObject.CurrX, PlayObject.CurrY, 9, TmpMerList) > 0)
                    {
                        for (var i = 0; i < TmpMerList.Count; i++)
                        {
                            Merchant = (Merchant)TmpMerList[i];
                            Merchant.ClearScript();
                            Merchant.LoadMerchantScript();
                            PlayObject.SysMsg(Merchant.ChrName + "重新加载成功...", MsgColor.Green, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("附近未发现任何交易NPC!!!", MsgColor.Red, MsgType.Hint);
                    }

                    IList<BaseObject> TmpNorList = new List<BaseObject>();
                    if (M2Share.WorldEngine.GetNpcList(PlayObject.Envir, PlayObject.CurrX, PlayObject.CurrY, 9, TmpNorList) > 0)
                    {
                        for (var i = 0; i < TmpNorList.Count; i++)
                        {
                            NPC = TmpNorList[i] as NormNpc;
                            NPC.ClearScript();
                            NPC.LoadNPCScript();
                            PlayObject.SysMsg(NPC.ChrName + "重新加载成功...", MsgColor.Green, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("附近未发现任何管理NPC!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                finally
                {
                }
            }
        }
    }
}