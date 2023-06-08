using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 解除指定玩家物品绑定属性
    /// </summary>
    [Command("UnBindUseItem", "解除指定玩家物品绑定属性", 10)]
    public class UnBindUseItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sItem = @params.Length > 1 ? @params[1] : "";
            var sType = @params.Length > 2 ? @params[2] : "";

            //PlayObject m_PlayObject;
            //TUserItem UserItem = null;
            //int nItem = M2Share.GetUseItemIdx(sItem);
            //int nBind = -1;
            //TItemBind ItemBind;
            //int nItemIdx;
            //int nMakeIdex;
            //string sBindName;
            //if ((sType).CompareTo(("帐号")) == 0)
            //{
            //    nBind = 0;
            //}
            //if ((sType).CompareTo(("人物")) == 0)
            //{
            //    nBind = 1;
            //}
            //if ((sType).CompareTo(("IP")) == 0)
            //{
            //    nBind = 2;
            //}
            //if ((sType).CompareTo(("死亡")) == 0) // 死亡不爆出
            //{
            //    nBind = 3;
            //}
            //if ((nItem < 0) || (nBind < 0) || (string.IsNullOrEmpty(sHumanName)) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (Settings.Config.boGMShowFailMsg)
            //    {
            //        PlayObject.SysMsg(string.Format(Settings.GameCommandParamUnKnow, this.Attributes.Name, Settings.GameCommandBindUseItemHelpMsg), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //    return;
            //}
            //m_PlayObject = M2Share.WorldEngine.GePlayObject(sHumanName);
            //if (m_PlayObject == null)
            //{
            //    PlayObject.SysMsg(string.Format(Settings.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //UserItem = m_PlayObject.UseItems[nItem];
            //if (UserItem.Index == 0)
            //{
            //    PlayObject.SysMsg(string.Format(Settings.GameCommandBindUseItemNoItemMsg, sHumanName, sItem), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //nItemIdx = UserItem.Index;
            //nMakeIdex = UserItem.MakeIndex;
            //switch (nBind)
            //{
            //    case 0:
            //        sBindName = m_PlayObject.m_sUserID;
            //        //M2Share.g_ItemBindAccount.__Lock();
            //        try
            //        {
            //            if (M2Share.g_ItemBindAccount.Count > 0)
            //            {
            //                for (int i = 0; i < M2Share.g_ItemBindAccount.Count; i++)
            //                {
            //                    ItemBind = M2Share.g_ItemBindAccount[i];
            //                    if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.nMakeIdex == nMakeIdex))
            //                    {
            //                        PlayObject.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), MsgColor.c_Red, MsgType.t_Hint);
            //                        return;
            //                    }
            //                }
            //            }
            //            ItemBind = new TItemBind();
            //            ItemBind.nItemIdx = nItemIdx;
            //            ItemBind.nMakeIdex = nMakeIdex;
            //            ItemBind.sBindName = sBindName;
            //            M2Share.g_ItemBindAccount.Insert(0, ItemBind);
            //        }
            //        finally
            //        {
            //            //M2Share.g_ItemBindAccount.UnLock();
            //        }
            //        M2Share.SaveItemBindAccount();
            //        PlayObject.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), MsgColor.c_Blue, MsgType.t_Hint);
            //        m_PlayObject.SysMsg(string.Format("您的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), sType, sBindName), MsgColor.c_Blue, MsgType.t_Hint);
            //        break;
            //    case 1:
            //        sBindName = m_PlayObject.m_sChrName;
            //        //M2Share.g_ItemBindChrName.__Lock();
            //        try
            //        {
            //            if (M2Share.g_ItemBindChrName.Count > 0)
            //            {
            //                for (int i = 0; i < M2Share.g_ItemBindChrName.Count; i++)
            //                {
            //                    //ItemBind = M2Share.g_ItemBindChrName[i];
            //                    //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.nMakeIdex == nMakeIdex))
            //                    //{
            //                    //    this.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), MsgColor.c_Red, MsgType.t_Hint);
            //                    //    return;
            //                    //}
            //                }
            //            }
            //            ItemBind = new TItemBind();
            //            ItemBind.nItemIdx = nItemIdx;
            //            ItemBind.nMakeIdex = nMakeIdex;
            //            ItemBind.sBindName = sBindName;
            //            // M2Share.g_ItemBindChrName.InsertText(0, ItemBind);
            //        }
            //        finally
            //        {
            //            //M2Share.g_ItemBindChrName.UnLock();
            //        }
            //        M2Share.SaveItemBindChrName();
            //        //this.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        //PlayObject.SysMsg(string.Format("您的%s[%s]已经绑定到%s[%s]上了。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), sType, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        break;
            //    case 2:
            //        sBindName = m_PlayObject.m_sIPaddr;
            //        //M2Share.g_ItemBindIPaddr.__Lock();
            //        try
            //        {
            //            if (M2Share.g_ItemBindIPaddr.Count > 0)
            //            {
            //                for (int i = 0; i < M2Share.g_ItemBindIPaddr.Count; i++)
            //                {
            //                    //ItemBind = M2Share.g_ItemBindIPaddr[i];
            //                    //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.nMakeIdex == nMakeIdex))
            //                    //{
            //                    //    this.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), MsgColor.c_Red, MsgType.t_Hint);
            //                    //    return;
            //                    //}
            //                }
            //            }
            //            ItemBind = new TItemBind();
            //            ItemBind.nItemIdx = nItemIdx;
            //            ItemBind.nMakeIdex = nMakeIdex;
            //            ItemBind.sBindName = sBindName;
            //            // M2Share.g_ItemBindIPaddr.InsertText(0, ItemBind);
            //        }
            //        finally
            //        {
            //            //M2Share.g_ItemBindIPaddr.UnLock();
            //        }
            //        M2Share.SaveItemBindIPaddr();
            //        //this.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        //PlayObject.SysMsg(string.Format("您的%s[%s]已经绑定到%s[%s]上了。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), sType, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        break;
            //    case 3:// 人物装备死亡不爆绑定
            //        sBindName = m_PlayObject.m_sChrName;
            //        HUtil32.EnterCriticalSection(Settings.g_ItemBindDieNoDropName);
            //        try
            //        {
            //            if (Settings.g_ItemBindDieNoDropName.Count > 0)
            //            {
            //                for (int i = 0; i < Settings.g_ItemBindDieNoDropName.Count; i++)
            //                {
            //                    //ItemBind = Settings.g_ItemBindDieNoDropName[i];
            //                    //if ((ItemBind.nItemIdx == nItemIdx) && (ItemBind.sBindName == sBindName))
            //                    //{
            //                    //    this.SysMsg(string.Format(Settings.GameCommandBindUseItemAlreadBindMsg, new string[] { sHumanName, sItem }), MsgColor.c_Red, MsgType.t_Hint);
            //                    //    return;
            //                    //}
            //                }
            //            }
            //            ItemBind = new TItemBind();
            //            ItemBind.nItemIdx = nItemIdx;
            //            ItemBind.nMakeIdex = 0;
            //            ItemBind.sBindName = sBindName;
            //            //Settings.g_ItemBindDieNoDropName.InsertText(0, ItemBind);
            //        }
            //        finally
            //        {
            //            HUtil32.LeaveCriticalSection(Settings.g_ItemBindDieNoDropName);
            //        }
            //        M2Share.SaveItemBindDieNoDropName();
            //        // 保存人物装备死亡不爆列表 20081127
            //        //this.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), UserItem.Index, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        //PlayObject.SysMsg(string.Format("您的%s[%s]已经绑定到%s[%s]上了。", new string[] { M2Share.GetUseItemName(nItem), ItemSystem.GetStdItemName(UserItem.Index), sType, sBindName }), MsgColor.c_Blue, MsgType.t_Hint);
            //        break;
            //}
        }
    }
}