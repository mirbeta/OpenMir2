using System;
using SystemModule;
using System.Collections.Generic;
using GameSvr.CommandSystem;
using System.Collections;

namespace GameSvr
{
    /// <summary>
    /// 重新加载当前12格范围内NPC
    /// </summary>
    [GameCommand("ReloadNpc", "重新加载当前9格范围内NPC", 10)]
    public class ReloadNpcCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadNpc(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = string.Empty;
            if (@Params != null)
            {
                sParam = @Params.Length > 0 ? @Params[0] : "";
            }
            IList<TBaseObject> TmpMerList = null;
            IList<TBaseObject> TmpNorList = null;
            TMerchant Merchant;
            TNormNpc NPC;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if (string.Compare("all".ToLower(), sParam.ToLower(), StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                //M2Share.ScriptSystem.ReLoadMerchants();
                M2Share.UserEngine.ReloadMerchantList();
                PlayObject.SysMsg("交易NPC重新加载完成!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                M2Share.UserEngine.ReloadNpcList();
                PlayObject.SysMsg("管理NPC重新加载完成!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TmpMerList = new List<TBaseObject>();
            try
            {
                if (M2Share.UserEngine.GetMerchantList(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 9, TmpMerList) > 0)
                {
                    for (var i = 0; i < TmpMerList.Count; i++)
                    {
                        Merchant = (TMerchant)TmpMerList[i];
                        Merchant.ClearScript();
                        Merchant.LoadNPCScript();
                        PlayObject.SysMsg(Merchant.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("附近未发现任何交易NPC!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
                TmpNorList = new List<TBaseObject>();
                if (M2Share.UserEngine.GetNpcList(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 9, TmpNorList) > 0)
                {
                    for (var i = 0; i < TmpNorList.Count; i++)
                    {
                        NPC = TmpNorList[i] as TNormNpc;
                        NPC.ClearScript();
                        NPC.LoadNPCScript();
                        PlayObject.SysMsg(NPC.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("附近未发现任何管理NPC!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            finally
            {
                TmpMerList = null;
                TmpNorList = null;
            }
        }
    }
}