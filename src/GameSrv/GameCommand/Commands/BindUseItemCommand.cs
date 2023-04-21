using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands
{
    [Command("BindUseItem", "", CommandHelp.GameCommandBindUseItemHelpMsg, 10)]
    public class BindUseItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sItem = @params.Length > 1 ? @params[1] : "";
            var sType = @params.Length > 2 ? @params[2] : "";
            var sLight = @params.Length > 3 ? @params[3] : "";
            var nBind = -1;
            var nItem = GameShare.GetUseItemIdx(sItem);
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
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null)
            {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var userItem = mPlayObject.UseItems[nItem];
            if (userItem.Index == 0)
            {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemNoItemMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nItemIdx = userItem.Index;
            var nMakeIdex = userItem.MakeIndex;
            ItemBind itemBind;
            string sBindName;
            bool boFind;
            switch (nBind)
            {
                case 0:
                    boFind = false;
                    sBindName = mPlayObject.UserAccount;
                    HUtil32.EnterCriticalSection(GameShare.ItemBindAccount);
                    try
                    {
                        for (var i = 0; i < GameShare.ItemBindAccount.Count; i++)
                        {
                            itemBind = GameShare.ItemBindAccount[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                playObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
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
                            GameShare.ItemBindAccount.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(GameShare.ItemBindAccount);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    GameShare.SaveItemBindAccount();
                    playObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mPlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    mPlayObject.SendMsg(playObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 1:
                    sBindName = mPlayObject.ChrName;
                    boFind = false;
                    HUtil32.EnterCriticalSection(GameShare.ItemBindChrName);
                    try
                    {
                        for (var i = 0; i < GameShare.ItemBindChrName.Count; i++)
                        {
                            itemBind = GameShare.ItemBindChrName[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                playObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
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
                            GameShare.ItemBindChrName.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(GameShare.ItemBindChrName);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    GameShare.SaveItemBindChrName();
                    playObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mPlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    playObject.SendUpdateItem(userItem);
                    mPlayObject.SendMsg(playObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 2:
                    boFind = false;
                    sBindName = mPlayObject.LoginIpAddr;
                    HUtil32.EnterCriticalSection(GameShare.ItemBindIPaddr);
                    try
                    {
                        for (var i = 0; i < GameShare.ItemBindIPaddr.Count; i++)
                        {
                            itemBind = GameShare.ItemBindIPaddr[i];
                            if (itemBind.nItemIdx == nItemIdx && itemBind.nMakeIdex == nMakeIdex)
                            {
                                playObject.SysMsg(string.Format(CommandHelp.GameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), MsgColor.Red, MsgType.Hint);
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
                            GameShare.ItemBindIPaddr.Insert(0, itemBind);
                        }
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(GameShare.ItemBindIPaddr);
                    }
                    if (boFind)
                    {
                        return;
                    }
                    GameShare.SaveItemBindIPaddr();
                    playObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，绑定到{6}成功。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    mPlayObject.SysMsg(string.Format("你的{0}[{1}]已经绑定到{2}[{3}]上了。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    playObject.SendUpdateItem(userItem);
                    mPlayObject.SendMsg(playObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                    break;
                case 3:// 人物装备死亡不爆绑定
                    sBindName = playObject.ChrName;
                    for (var i = 0; i < GameShare.ItemBindDieNoDropName.Count; i++)
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
                    mPlayObject.SysMsg(string.Format("{0}[{1}]IDX[{2}]系列号[{3}]持久[{4}-{5}]，死亡不爆绑定到{6}成功。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), userItem.Index, userItem.MakeIndex, userItem.Dura, userItem.DuraMax, sBindName), MsgColor.Blue, MsgType.Hint);
                    playObject.SysMsg(string.Format("您的{0}[{1}]已经绑定到{2}[{3}]上了。", GameShare.GetUseItemName(nItem), GameShare.WorldEngine.GetStdItemName(userItem.Index), sType, sBindName), MsgColor.Blue, MsgType.Hint);
                    break;
            }
        }
    }
}