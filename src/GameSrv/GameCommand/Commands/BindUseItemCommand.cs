using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    [Command("BindUseItem", "", CommandHelp.GameCommandBindUseItemHelpMsg, 10)]
    public class BindUseItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null || @Params.Length <= 0) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sItem = @Params.Length > 1 ? @Params[1] : "";
            string sType = @Params.Length > 2 ? @Params[2] : "";
            string sLight = @Params.Length > 3 ? @Params[3] : "";
            int nBind = -1;
            int nItem = M2Share.GetUseItemIdx(sItem);
            if (string.Compare(sType, "帐号", StringComparison.OrdinalIgnoreCase) == 0) {
                nBind = 0;
            }
            if (string.Compare(sType, "人物", StringComparison.OrdinalIgnoreCase) == 0) {
                nBind = 1;
            }
            if (string.Compare(sType, "IP", StringComparison.OrdinalIgnoreCase) == 0) {
                nBind = 2;
            }
            if (string.Compare(sType, "死亡", StringComparison.OrdinalIgnoreCase) == 0) {
                nBind = 3;
            }
            bool boLight = sLight == "1";
            if (nItem < 0 || nBind < 0 || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            UserItem UserItem = m_PlayObject.UseItems[nItem];
            if (UserItem.Index == 0) {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemNoItemMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemIdx = UserItem.Index;
            int nMakeIdex = UserItem.MakeIndex;
            ItemBind ItemBind;
            string sBindName;
            bool boFind;
            switch (nBind) {
                case 0:
                    boFind = false;
                    sBindName = m_PlayObject.UserAccount;
                    HUtil32.EnterCriticalSection(M2Share.ItemBindAccount);
                    try {
                        for (int i = 0; i < M2Share.ItemBindAccount.Count; i++) {
                            ItemBind = M2Share.ItemBindAccount[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex) {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind) {
                            ItemBind = new ItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.ItemBindAccount.Insert(0, ItemBind);
                        }
                    }
                    finally {
                        HUtil32.LeaveCriticalSection(M2Share.ItemBindAccount);
                    }
                    if (boFind) {
                        return;
                    }
                    M2Share.SaveItemBindAccount();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SendMsg(m_PlayObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 1:
                    sBindName = m_PlayObject.ChrName;
                    boFind = false;
                    HUtil32.EnterCriticalSection(M2Share.ItemBindChrName);
                    try {
                        for (int i = 0; i < M2Share.ItemBindChrName.Count; i++) {
                            ItemBind = M2Share.ItemBindChrName[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex) {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind) {
                            ItemBind = new ItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.ItemBindChrName.Insert(0, ItemBind);
                        }
                    }
                    finally {
                        HUtil32.LeaveCriticalSection(M2Share.ItemBindChrName);
                    }
                    if (boFind) {
                        return;
                    }
                    M2Share.SaveItemBindChrName();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SendUpdateItem(UserItem);
                    m_PlayObject.SendMsg(m_PlayObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 2:
                    boFind = false;
                    sBindName = m_PlayObject.LoginIpAddr;
                    HUtil32.EnterCriticalSection(M2Share.ItemBindIPaddr);
                    try {
                        for (int i = 0; i < M2Share.ItemBindIPaddr.Count; i++) {
                            ItemBind = M2Share.ItemBindIPaddr[i];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex) {
                                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind) {
                            ItemBind = new ItemBind();
                            ItemBind.nItemIdx = nItemIdx;
                            ItemBind.nMakeIdex = nMakeIdex;
                            ItemBind.sBindName = sBindName;
                            M2Share.ItemBindIPaddr.Insert(0, ItemBind);
                        }
                    }
                    finally {
                        HUtil32.LeaveCriticalSection(M2Share.ItemBindIPaddr);
                    }
                    if (boFind) {
                        return;
                    }
                    M2Share.SaveItemBindIPaddr();
                    PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    m_PlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SendUpdateItem(UserItem);
                    m_PlayObject.SendMsg(m_PlayObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                    break;
                case 3:// 人物装备死亡不爆绑定
                    sBindName = PlayObject.ChrName;
                    for (int i = 0; i < M2Share.ItemBindDieNoDropName.Count; i++) {
                        //ItemBind = Settings.g_ItemBindDieNoDropName[i];
                        //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.sBindName == sBindName))
                        //{
                        //    this.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), TMsgColor.c_Red, TMsgType.t_Hint);
                        //    return;
                        //}
                    }
                    ItemBind = new ItemBind {
                        nItemIdx = nItemIdx,
                        nMakeIdex = 0,
                        sBindName = sBindName
                    };
                    //Settings.g_ItemBindDieNoDropName.InsertText(0, ItemBind);
                    //M2Share.SaveItemBindDieNoDropName();// 保存人物装备死亡不爆列表
                    m_PlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，死亡不爆绑定到{6}成功。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayObject.SysMsg(string.Format("您的{0}[{1}]已经绑定到{2}[{3}]上了。", M2Share.GetUseItemName(nItem), M2Share.WorldEngine.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    break;
            }
        }
    }
}