using GameSvr.GameCommand;
using GameSvr.Items;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    /// <summary>
    /// 元宝寄售相关
    /// </summary>
    public partial class PlayObject
    {
        /// <summary>
        /// 人物上线,检查是否有交易结束还没得到元宝 
        /// 交易成功后修改数据标识
        /// </summary>
        private void UpdateSellOffInfo(int code)
        {
            if (BoYbDeal)// 已开通元宝服务
            {
                for (int i = M2Share.SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    var dealOffInfo = M2Share.SellOffItemList[i];
                    if (dealOffInfo != null)
                    {
                        if (dealOffInfo.Flag == 2)
                        {
                            switch (code)
                            {
                                case 0: // 出售者
                                    if (dealOffInfo.sDealChrName == this.ChrName)
                                    {
                                        M2Share.SellOffItemList.RemoveAt(i);
                                        Dispose(dealOffInfo);
                                    }
                                    break;
                                case 1: // 购买者
                                    if (dealOffInfo.sBuyChrName == this.ChrName)
                                    {
                                        M2Share.SellOffItemList.RemoveAt(i);
                                        Dispose(dealOffInfo);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 客户端增加寄售物品到出售框中
        /// </summary>
        private void ClientAddSellOffItem(int nItemIdx, string sItemName)
        {
            if (nItemIdx <= 0 || string.IsNullOrEmpty(sItemName))
            {
                this.SendMsg(this, Messages.RM_SellOffADDITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            bool bo11;
            var sUserItemName = string.Empty;
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, ' ');
            }
            bo11 = false;
            if (!SellOffConfirm)
            {
                for (var i = this.ItemList.Count - 1; i >= 0; i--)
                {
                    if (this.ItemList.Count <= 0)
                    {
                        break;
                    }
                    UserItem UserItem = this.ItemList[i];
                    if (UserItem == null)
                    {
                        continue;
                    }
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        // 取自定义物品名称
                        if (UserItem.Desc[13] == 1)
                        {
                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(UserItem.MakeIndex, UserItem.Index);
                        }
                        if (string.IsNullOrEmpty(sUserItemName))
                        {
                            sUserItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                        }
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0 && SellOffItemList.Count < 9)
                        {
                            //if (this.CheckItemValue(UserItem, 1))
                            //{
                            //    break;
                            //}
                            //else if (this.PlugOfCheckCanItem(1, sUserItemName, false, 0, 0)) // 禁止物品规则(管理插件功能)
                            //{
                            //    break;
                            //}
                            SellOffItemList.Add(UserItem);
                            this.SendMsg(this, Messages.RM_SELLOFFADDITEM_OK, 0, 0, 0, 0, ""); // 放物品成功
                            this.ItemList.RemoveAt(i);
                            //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                this.SendMsg(this, Messages.RM_SellOffADDITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        /// <summary>
        /// 客户端删除出售物品窗里的物品
        /// </summary>
        private void ClientDelSellOffItem(int nItemIdx, string sItemName)
        {
            if (nItemIdx <= 0 || string.IsNullOrEmpty(sItemName))
            {
                this.SendMsg(this, Messages.RM_SELLOFFDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            string sUserItemName = string.Empty;
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, ' ');
            }
            bool bo11 = false;
            if (!SellOffConfirm)
            {
                for (int i = SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    UserItem UserItem = SellOffItemList[i];
                    if (UserItem == null)
                    {
                        continue;
                    }
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        if (UserItem.Desc[13] == 1)
                        {
                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(UserItem.MakeIndex, UserItem.Index); // 取自定义物品名称
                        }
                        if (string.IsNullOrEmpty(sUserItemName))
                        {
                            sUserItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                        }
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品 
                            this.ItemList.Add(UserItem);
                            this.SendMsg(this, Messages.RM_SELLOFFDELITEM_OK, 0, 0, 0, 0, "");
                            SellOffItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                this.SendMsg(this, Messages.RM_SELLOFFDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        /// <summary>
        /// 出售人取消正在出售中的交易
        /// </summary>
        private void ClientCancelSellOffIng()
        {
            try
            {
                if (M2Share.SellOffItemList == null || M2Share.SellOffItemList.Count == 0 || !IsEnoughBag())
                {
                    return;
                }
                for (int i = M2Share.SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    var DealOffInfo = M2Share.SellOffItemList[i];
                    if (DealOffInfo != null)
                    {
                        if (string.Compare(DealOffInfo.sDealChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (DealOffInfo.Flag == 0 || DealOffInfo.Flag == 3))
                        {
                            DealOffInfo.Flag = 4;
                            for (var j = 0; j < 9; j++)
                            {
                                if (DealOffInfo.UseItems[j] == null)
                                {
                                    continue;
                                }
                                var StdItem = M2Share.WorldEngine.GetStdItem(DealOffInfo.UseItems[j].Index);
                                if (StdItem != null)
                                {
                                    //UserItem = new TUserItem();
                                    var UserItem = DealOffInfo.UseItems[j];
                                    if (IsEnoughBag())// 人物的包裹是否满了
                                    {
                                        if (this.IsAddWeightAvailable(StdItem.Weight))// 检查负重
                                        {
                                            if (this.AddItemToBag(UserItem))
                                            {
                                                SendAddItem(UserItem);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.DropItemDown(UserItem, 3, false, this.ActorId, this.ActorId);
                                    }
                                }
                                // 是金刚石
                                else if (DealOffInfo.UseItems[j].MakeIndex > 0 && DealOffInfo.UseItems[j].Index == short.MaxValue && DealOffInfo.UseItems[j].Dura == short.MaxValue && DealOffInfo.UseItems[j].DuraMax == short.MaxValue)
                                {
                                    Gold += DealOffInfo.UseItems[j].MakeIndex; // 增加金刚石
                                    this.GameGoldChanged(); // 更新金刚石数量
                                }
                            }
                            M2Share.SellOffItemList.RemoveAt(i);
                            Dispose(DealOffInfo);
                            this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "取消寄售成功!");
                            M2Share.CommonDb.SaveSellOffItemList();//保存元宝寄售列表
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TPlayObject.ClientCancelSellOffIng");
            }
        }

        /// <summary>
        /// 购买人取消交易
        /// </summary>
        private void ClientBuyCancelSellOff(string dealChrName)
        {
            for (int i = M2Share.SellOffItemList.Count - 1; i >= 0; i--)
            {
                if (M2Share.SellOffItemList.Count <= 0)
                {
                    break;
                }
                DealOffInfo dealOffInfo = M2Share.SellOffItemList[i];
                if (dealOffInfo != null)
                {
                    if (string.Compare(dealOffInfo.sDealChrName, dealChrName, StringComparison.OrdinalIgnoreCase) == 0 &&
                        string.Compare(dealOffInfo.sBuyChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 0)
                    {
                        dealOffInfo.Flag = 3;// 购买人取消标识
                        // sSellOffItemList.Delete(I);
                        // sSellOffItemList.Add(DealOffInfo);
                        this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "取消交易成功!");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 购买寄售物品
        /// </summary>
        private void ClientBuySellOffItme(string dealChrName)
        {
            try
            {
                for (int i = M2Share.SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    DealOffInfo dealOffInfo = M2Share.SellOffItemList[i];
                    if (dealOffInfo != null)
                    {
                        if (string.Compare(dealOffInfo.sDealChrName, dealChrName, StringComparison.OrdinalIgnoreCase) == 0
                            && string.Compare(dealOffInfo.sBuyChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 0)
                        {
                            dealOffInfo.Flag = 4;
                            if (GameGold >= dealOffInfo.nSellGold + M2Share.Config.DecUserGameGold)// 每次扣多少元宝(元宝寄售)
                            {
                                GameGold -= dealOffInfo.nSellGold + M2Share.Config.DecUserGameGold; // 扣出元宝
                                if (GameGold < 0)
                                {
                                    GameGold = 0;
                                }
                                this.GameGoldChanged(); // 更新元宝数量
                                var PlayObject = M2Share.WorldEngine.GetPlayObject(dealOffInfo.sDealChrName);
                                if (PlayObject == null)// 出售人不在线
                                {
                                    dealOffInfo.Flag = 1; // 物品已出售,出售人未得到元宝
                                    // sSellOffItemList.Delete(I);
                                    // sSellOffItemList.Add(DealOffInfo);
                                }
                                else
                                {
                                    if (PlayObject.OffLineFlag)  // 挂机
                                    {
                                        dealOffInfo.Flag = 1; // 物品已出售,出售人未得到元宝
                                    }
                                    else
                                    {
                                        UpdateSellOffInfo(1);
                                        dealOffInfo.Flag = 2; // 交易结束
                                        PlayObject.GameGold += dealOffInfo.nSellGold;
                                        PlayObject.GameGoldChanged();
                                        PlayObject.SysMsg(string.Format(CommandHelp.GetSellOffGlod, new object[] { dealOffInfo.nSellGold, M2Share.Config.GameGoldName }), MsgColor.Red, MsgType.Hint);
                                        if (M2Share.GameLogGameGold)
                                        {
                                            M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, PlayObject.MapName, PlayObject.CurrX, PlayObject.CurrY, PlayObject.ChrName, M2Share.Config.GameGoldName, PlayObject.GameGold, "寄售获得(" + dealOffInfo.nSellGold + ')', this.ChrName));
                                        }
                                    }
                                }
                                M2Share.CommonDb.SaveSellOffItemList();//保存元宝寄售列表
                                for (int j = 0; j <= 9; j++)
                                {
                                    var StdItem = M2Share.WorldEngine.GetStdItem(dealOffInfo.UseItems[j].Index);
                                    if (StdItem != null)
                                    {
                                        //UserItem = new TUserItem();
                                        var UserItem = dealOffInfo.UseItems[j];
                                        if (IsEnoughBag()) // 检查人物的包裹是否满了 
                                        {
                                            //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品 
                                            if (this.AddItemToBag(UserItem))
                                            {
                                                SendAddItem(UserItem);
                                                if (StdItem.NeedIdentify == 1)
                                                {
                                                    // M2Share.ItemEventSource.AddGameLog('9' + "\09" + this.m_sMapName + "(*)" + "\09" + this.m_nCurrX.ToString() + "\09" + this.m_nCurrY.ToString() + "\09" + this.m_sChrName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex.ToString() + "\09" + '(' + HUtil32.LoWord(StdItem.DC).ToString() + '/' + HUtil32.HiWord(StdItem.DC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.MC).ToString() + '/' + HUtil32.HiWord(StdItem.MC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.SC).ToString() + '/' + HUtil32.HiWord(StdItem.SC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.AC).ToString() + '/' + HUtil32.HiWord(StdItem.AC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.MAC).ToString() + '/' + HUtil32.HiWord(StdItem.MAC).ToString() + ')' + UserItem.btValue[0].ToString() + '/' + UserItem.btValue[1].ToString() + '/' + UserItem.btValue[2].ToString() + '/' + UserItem.btValue[3].ToString() + '/' + UserItem.btValue[4].ToString() + '/' + UserItem.btValue[5].ToString() + '/' + UserItem.btValue[6].ToString() + '/' + UserItem.btValue[7].ToString() + '/' + UserItem.btValue[8].ToString() + '/' + UserItem.btValue[14].ToString() + "\09" + DealOffInfo.sDealChrName);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.DropItemDown(UserItem, 3, false, this.ActorId, this.ActorId);
                                        }
                                    }
                                    // 是金刚石
                                    else if (dealOffInfo.UseItems[j].MakeIndex > 0 && dealOffInfo.UseItems[j].Index == short.MaxValue && dealOffInfo.UseItems[j].Dura == short.MaxValue && dealOffInfo.UseItems[j].DuraMax == short.MaxValue)
                                    {
                                        Gold += dealOffInfo.UseItems[j].MakeIndex; // 增加金刚石
                                        this.SysMsg(dealOffInfo.UseItems[j].MakeIndex + " 颗金刚石增加", MsgColor.Blue, MsgType.Hint);
                                    }
                                }
                                this.SendMsg(this, Messages.RM_SELLOFFBUY_OK, 0, 0, 0, 0, "");// 购买成功
                                this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "[成功] 系统已经成功接受您的申请");
                                break;
                            }
                            else
                            {
                                dealOffInfo.Flag = 0;
                                this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "[错误] 您的申请提交不成功");
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TPlayObject.ClientBuySellOffItme");
            }
        }

        /// <summary>
        /// 人物上线,检查是否有交易结束还没得到元宝 
        /// </summary>
        private void GetSellOffGlod()
        {
            //if (m_boNotOnlineAddExp) // 挂机则退出  
            //{
            //    return;
            //}
            try
            {
                for (int i = M2Share.SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    DealOffInfo dealOffInfo = M2Share.SellOffItemList[i];
                    if (dealOffInfo != null)
                    {
                        if (string.Compare(dealOffInfo.sDealChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 1)
                        {
                            UpdateSellOffInfo(0);
                            dealOffInfo.Flag = 2; // 交易结束
                            // sSellOffItemList.Delete(I);
                            // sSellOffItemList.Add(DealOffInfo);
                            GameGold += dealOffInfo.nSellGold;
                            this.GameGoldChanged();
                            this.SysMsg(string.Format(CommandHelp.GetSellOffGlod, new object[] { dealOffInfo.nSellGold, M2Share.Config.GameGoldName }), MsgColor.Red, MsgType.Hint);
                            if (M2Share.GameLogGameGold)
                            {
                                M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, this.MapName, this.CurrX, this.CurrY, this.ChrName, M2Share.Config.GameGoldName, GameGold, "寄售获得(" + dealOffInfo.nSellGold + ')', dealOffInfo.sBuyChrName));
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TPlayObject.GetSellOffGlod");
            }
        }

        /// <summary>
        /// 客户端取消元宝寄售
        /// </summary>
        private void ClientCancelSellOff()
        {
            SellOffCancel();
        }

        /// <summary>
        /// 查询玩家交易记录
        /// </summary>
        /// <returns></returns>
        public string SelectSellDate()
        {
            string result = "您未开通" + M2Share.Config.GameGoldName + "寄售服务,请先开通!!!\\ \\<返回/@main>";
            if (BoYbDeal)
            {
                // 已开通元宝服务
                if (M2Share.SellOffItemList.Count > 0)
                {
                    for (int i = 0; i < M2Share.SellOffItemList.Count; i++)
                    {
                        DealOffInfo dealOffInfo = M2Share.SellOffItemList[i];
                        if (dealOffInfo != null)
                        {
                            if (string.Compare(dealOffInfo.sDealChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 2)
                            {
                                result = "最后一笔出售记录:\\   " + dealOffInfo.dSellDateTime.ToString("yyyy年mm月dd日 hh时ss分") + ",\\  您与" + dealOffInfo.sBuyChrName + "交易成功,获得了" + dealOffInfo.nSellGold + '个' + M2Share.Config.GameGoldName + "。\\ \\<返回/@main>";
                                return result;
                            }
                            else if (string.Compare(dealOffInfo.sBuyChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (dealOffInfo.Flag == 1 || dealOffInfo.Flag == 2))
                            {
                                result = "最后一笔购买记录:\\   " + dealOffInfo.dSellDateTime.ToString("yyyy年mm月dd日 hh时ss分") + ",\\  您与" + dealOffInfo.sDealChrName + "交易成功,支付了" + dealOffInfo.nSellGold + '个' + M2Share.Config.GameGoldName + "。\\ \\<返回/@main>";
                                return result;
                            }
                        }
                    }
                }
                result = "您未进行任何寄售交易!!!\\ \\<返回/@main>";
            }
            return result;
        }

        /// <summary>
        /// 查询玩家是否操作过寄售
        /// </summary>
        /// <returns></returns>
        public bool SellOffInTime(int nCode)
        {
            bool result = false;
            if (M2Share.SellOffItemList.Count > 0)
            {
                for (int i = 0; i < M2Share.SellOffItemList.Count; i++)
                {
                    DealOffInfo dealOffInfo = M2Share.SellOffItemList[i];
                    if (dealOffInfo != null)
                    {
                        switch (nCode)
                        {
                            case 0: // 出售者
                                if (string.Compare(dealOffInfo.sDealChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (dealOffInfo.Flag == 0 || dealOffInfo.Flag == 3))
                                {
                                    result = true;
                                    break;
                                }
                                break;
                            case 1: // 购买者
                                if (string.Compare(dealOffInfo.sBuyChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 0)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 客户端元宝寄售结束
        /// 把临时列表数据写入元宝寄售列表中,并清空临时列表
        /// </summary>
        /// <param name="sBuyChrName">买方</param>
        /// <param name="nSellGold">元宝数</param>
        /// <param name="nGameDiamond">金刚石数</param>
        /// <param name="nCode">金刚石特征,类型上限表示</param>
        private void ClientSellOffEnd(string sBuyChrName, int nSellGold, int nGameDiamond, int nCode)
        {
            UserItem UserItem;
            StdItem StdItem;
            DealOffInfo DealOffInfo;
            SellOffConfirm = true;
            bool bo11 = false;
            if (SellOffConfirm && (SellOffItemList.Count > 0 || nGameDiamond > 0) && SellOffItemList.Count < 10 && sBuyChrName.Length < 20 && nSellGold > 0 && nSellGold < 100000000
                && string.Compare(sBuyChrName, this.ChrName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                // 不能自己寄售给自己
                DealOffInfo = new DealOffInfo() { UseItems = new UserItem[9] };
                if (SellOffItemList.Count > 0)
                {
                    for (int i = 0; i < SellOffItemList.Count; i++)
                    {
                        UserItem = SellOffItemList[i];
                        StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null && UserItem != null && UserItem.MakeIndex > 0)
                        {
                            DealOffInfo.UseItems[i] = UserItem;
                        }
                    }
                }
                for (int j = 0; j < 9; j++)
                {
                    if (DealOffInfo.UseItems[j] == null)
                    {
                        continue;
                    }
                    StdItem = M2Share.WorldEngine.GetStdItem(DealOffInfo.UseItems[j].Index);
                    if (StdItem == null && nGameDiamond > 0 && nGameDiamond < 10000 && nCode == short.MaxValue)// 物品是金刚石
                    {
                        if (nGameDiamond > Gold) // 金刚石数量大于玩家的数量时则反回失败
                        {
                            this.SendMsg(this, Messages.RM_SELLOFFEND_FAIL, 0, 0, 0, 0, "");
                            this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "[错误] 你没有那么多金币");
                            GetBackSellOffItems(); // 返回物品
                            return;
                        }
                        Gold -= nGameDiamond;
                        this.GameGoldChanged(); // 更新金刚石数量
                        DealOffInfo.UseItems[j].MakeIndex = nGameDiamond; // 金刚石数量
                        DealOffInfo.UseItems[j].Index = ushort.MaxValue;
                        DealOffInfo.UseItems[j].Dura = ushort.MaxValue;
                        DealOffInfo.UseItems[j].DuraMax = ushort.MaxValue;
                        break;
                    }
                }
                DealOffInfo.sDealChrName = this.ChrName; // 寄售人
                DealOffInfo.sBuyChrName = sBuyChrName.Trim(); // 购买人
                DealOffInfo.nSellGold = nSellGold; // 元宝数
                DealOffInfo.dSellDateTime = DateTime.Now; // 操作时间
                DealOffInfo.Flag = 0; // 标识
                M2Share.SellOffItemList.Add(DealOffInfo); // 增加到元宝寄售列表中
                this.SendMsg(this, Messages.RM_SELLOFFEND_OK, 0, 0, 0, 0, "");
                GameGold -= M2Share.Config.DecUserGameGold; // 每次扣多少元宝(元宝寄售) 
                if (GameGold < 0)
                {
                    GameGold = 0;
                }
                this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "[成功] 系统已经成功接受您的申请");
                bo11 = true;
                M2Share.CommonDb.SaveSellOffItemList();//保存元宝寄售列表 
                SellOffConfirm = false;
                SellOffItemList.Clear();
            }
            if (!bo11)
            {
                // 失败则返回物品给玩家
                this.SendMsg(this, Messages.RM_SELLOFFEND_FAIL, 0, 0, 0, 0, "");
                this.SendMsg(this, Messages.RM_MENU_OK, 0, this.ActorId, 0, 0, "[错误:] 寄售物品失败");
                GetBackSellOffItems();
            }
        }

        /// <summary>
        /// 元宝寄售取消出售
        /// </summary>
        private void SellOffCancel()
        {
            this.SendMsg(this, Messages.RM_SELLOFFCANCEL, 0, 0, 0, 0, "");
            GetBackSellOffItems();
        }

        /// <summary>
        /// 取备份元宝寄售列表物品
        /// </summary>
        public void GetBackSellOffItems()
        {
            if (SellOffItemList == null)
            {
                SellOffItemList = new List<UserItem>();
            }
            if (SellOffItemList.Count > 0)
            {
                for (int i = SellOffItemList.Count - 1; i >= 0; i--)
                {
                    this.ItemList.Add(SellOffItemList[i]);
                    SellOffItemList.RemoveAt(i);
                }
            }
            SellOffConfirm = false;// 确认元宝寄售标志 
        }
    }
}