using OpenMir2;
using OpenMir2.Data;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("BindUseItem", "", CommandHelp.GameCommandBindUseItemHelpMsg, 10)]
    public class BindUseItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sItem = @params.Length > 1 ? @params[1] : "";
            string sType = @params.Length > 2 ? @params[2] : "";
            string sLight = @params.Length > 3 ? @params[3] : "";
            int nBind = -1;
            int nItem = SystemShare.GetUseItemIdx(sItem);
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
            bool boLight = sLight == "1";
            if (nItem < 0 || nBind < 0 || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            OpenMir2.Packets.ClientPackets.UserItem userItem = mIPlayerActor.UseItems[nItem];
            if (userItem.Index == 0)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemNoItemMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemIdx = userItem.Index;
            int nMakeIdex = userItem.MakeIndex;
            ItemBind itemBind;
            string sBindName;
            bool boFind;
            switch (nBind)
            {
                case 0:
                    boFind = false;
                    sBindName = mIPlayerActor.UserAccount;
                    HUtil32.EnterCriticalSection(SystemShare.ItemBindAccount);
                    try
                    {
                        for (int i = 0; i < SystemShare.ItemBindAccount.Count; i++)
                        {
                            itemBind = SystemShare.ItemBindAccount[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            itemBind = new ItemBind();
                            itemBind.nItemIdx = nItemIdx;
                            itemBind.nMakeIdex = nMakeIdex;
                            itemBind.sBindName = sBindName;
                            SystemShare.ItemBindAccount.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(SystemShare.ItemBindAccount);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    SystemShare.SaveItemBindAccount();
                    PlayerActor.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mIPlayerActor.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    mIPlayerActor.SendMsg(PlayerActor, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 1:
                    sBindName = mIPlayerActor.ChrName;
                    boFind = false;
                    HUtil32.EnterCriticalSection(SystemShare.ItemBindChrName);
                    try
                    {
                        for (int i = 0; i < SystemShare.ItemBindChrName.Count; i++)
                        {
                            itemBind = SystemShare.ItemBindChrName[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            itemBind = new ItemBind();
                            itemBind.nItemIdx = nItemIdx;
                            itemBind.nMakeIdex = nMakeIdex;
                            itemBind.sBindName = sBindName;
                            SystemShare.ItemBindChrName.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(SystemShare.ItemBindChrName);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    SystemShare.SaveItemBindChrName();
                    PlayerActor.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mIPlayerActor.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayerActor.SendUpdateItem(userItem);
                    mIPlayerActor.SendMsg(PlayerActor, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 2:
                    boFind = false;
                    sBindName = mIPlayerActor.LoginIpAddr;
                    HUtil32.EnterCriticalSection(SystemShare.ItemBindIPaddr);
                    try
                    {
                        for (int i = 0; i < SystemShare.ItemBindIPaddr.Count; i++)
                        {
                            itemBind = SystemShare.ItemBindIPaddr[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                                boFind = true;
                                break;
                            }
                        }
                        if (!boFind)
                        {
                            itemBind = new ItemBind();
                            itemBind.nItemIdx = nItemIdx;
                            itemBind.nMakeIdex = nMakeIdex;
                            itemBind.sBindName = sBindName;
                            SystemShare.ItemBindIPaddr.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(SystemShare.ItemBindIPaddr);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    SystemShare.SaveItemBindIPaddr();
                    PlayerActor.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mIPlayerActor.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayerActor.SendUpdateItem(userItem);
                    mIPlayerActor.SendMsg(PlayerActor, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 3:// 人物装备死亡不爆绑定
                    sBindName = PlayerActor.ChrName;
                    for (int i = 0; i < SystemShare.ItemBindDieNoDropName.Count; i++)
                    {
                        //ItemBind = Settings.g_ItemBindDieNoDropName[i];
                        //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.sBindName == sBindName))
                        //{
                        //    this.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), MsgColor.c_Red, MsgType.t_Hint);
                        //    return;
                        //}
                    }
                    itemBind = new ItemBind
                    {
                        nItemIdx = nItemIdx,
                        nMakeIdex = 0,
                        sBindName = sBindName
                    };
                    //Settings.g_ItemBindDieNoDropName.InsertText(0, ItemBind);
                    //M2Share.SaveItemBindDieNoDropName();// 保存人物装备死亡不爆列表
                    mIPlayerActor.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，死亡不爆绑定到{6}成功。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    PlayerActor.SysMsg(string.Format("您的{0}[{1}]已经绑定到{2}[{3}]上了。", SystemShare.GetUseItemName(nItem), SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    break;
            }
        }
    }
}