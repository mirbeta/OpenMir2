using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.GameCommand.Commands
{
    [Command("BindUseItem", "", CommandHelp.GameCommandBindUseItemHelpMsg, 10)]
    public class BindUseItemCommand : Command
    {
        [ExecuteCommand]
        public void BindUseItem(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null || @Params.Length <= 0)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sItem = @Params.Length > 1 ? @Params[1] : "";
            var sType = @Params.Length > 2 ? @Params[2] : "";
            var sLight = @Params.Length > 3 ? @Params[3] : "";
            var nBind = -1;
            var nItem = M2Share.GetUseItemIdx(sItem);
            if (string.Compare(sType, "帐号", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nBind = 0;
            }
            if (string.Compare(sType, "人物", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nBind = 1;
            }
            if (string.Compare(sType, "IP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nBind = 2;
            }
            if (string.Compare(sType, "死亡", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nBind = 3;
            }
            var boLight = sLight == "1";
            if (nItem < 0 || nBind < 0 || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            UserItem UserItem = m_PlayObject.UseItems[nItem];
            if (UserItem.Index == 0)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemNoItemMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemIdx = UserItem.Index;
            var nMakeIdex = UserItem.MakeIndex;
            TItemBind ItemBind;
            string sBindName;
            bool boFind;
            switch (nBind)
            {
                case 0:
                    boFind = false;
                    sBindName = m_PlayObject.UserID;
                    HUtil32.EnterCriticalSection(M2Share.g_ItemBindAccount);
                    try
                    {
                        for (var i = 0; i < M2Share.g_ItemBindAccount.Count; i++)
                        {
                            ItemBind = M2Share.g_ItemBindAccount[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            ItemBind = new TItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.g_ItemBindAccount.Insert(0, ItemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(M2Share.g_ItemBindAccount);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    M2Share.SaveItemBindAccount();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 1:
                    sBindName = m_PlayObject.ChrName;
                    boFind = false;
                    HUtil32.EnterCriticalSection(M2Share.g_ItemBindChrName);
                    try
                    {
                        for (var i = 0; i < M2Share.g_ItemBindChrName.Count; i++)
                        {
                            ItemBind = M2Share.g_ItemBindChrName[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            ItemBind = new TItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.g_ItemBindChrName.Insert(0, ItemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(M2Share.g_ItemBindChrName);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    M2Share.SaveItemBindChrName();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SendUpdateItem(UserItem);
                    m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 2:
                    boFind = false;
                    sBindName = m_PlayObject.m_sIPaddr;
                    HUtil32.EnterCriticalSection(M2Share.g_ItemBindIPaddr);
                    try
                    {
                        for (var i = 0; i < M2Share.g_ItemBindIPaddr.Count; i++)
                        {
                            ItemBind = M2Share.g_ItemBindIPaddr[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            ItemBind = new TItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.g_ItemBindIPaddr.Insert(0, ItemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(M2Share.g_ItemBindIPaddr);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    M2Share.SaveItemBindIPaddr();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SendUpdateItem(UserItem);
                    m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 3:// 人物装备死亡不爆绑定
                    sBindName = PlayObject.ChrName;
                    for (var i = 0; i < M2Share.g_ItemBindDieNoDropName.Count; i++)
                    {
                        //ItemBind = M2Share.g_ItemBindDieNoDropName[i];
                        //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.sBindName == sBindName))
                        //{
                        //    this.SysMsg(string.Format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), TMsgColor.c_Red, TMsgType.t_Hint);
                        //    return;
                        //}
                    }
                    ItemBind = new TItemBind
                    {
                        nItemIdx = nItemIdx,
                        nMakeIdex = 0,
                        sBindName = sBindName
                    };
                    //M2Share.g_ItemBindDieNoDropName.InsertText(0, ItemBind);
                    //M2Share.SaveItemBindDieNoDropName();// 保存人物装备死亡不爆列表
                    m_PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，死亡不爆绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SysMsg(string.Format("您的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    break;
            }
        }
    }
}