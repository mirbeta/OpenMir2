using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemModule;

namespace M2Server
{
    /// <summary>
    /// 跨服元宝寄售系统
    /// </summary>
    public partial class TPlayObject
    {
        // 人物上线,检查是否有交易结束还没得到元宝 
        // 交易成功后修改数据标识 
        private void UpdateSellOffInfo(int code)
        {
            TDealOffInfo DealOffInfo;
            if (bo_YBDEAL)// 已开通元宝服务
            {
                for (var i = M2Share.sSellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.sSellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    DealOffInfo = M2Share.sSellOffItemList[i];
                    if (DealOffInfo != null)
                    {
                        if (DealOffInfo.N == 2)
                        {
                            switch (code)
                            {
                                case 0: // 出售者
                                    if (DealOffInfo.sDealCharName == this.m_sCharName)
                                    {
                                        M2Share.sSellOffItemList.RemoveAt(i);
                                        Dispose(DealOffInfo);
                                        break;
                                    }
                                    break;
                                case 1: // 购买者
                                    if (DealOffInfo.sBuyCharName == this.m_sCharName)
                                    {
                                        M2Share.sSellOffItemList.RemoveAt(i);
                                        Dispose(DealOffInfo);
                                        break;
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
        /// <param name="nItemIdx"></param>
        /// <param name="sItemName"></param>        
        private void ClientAddSellOffItem(int nItemIdx, string sItemName)
        {
            bool bo11;
            TUserItem UserItem;
            string sUserItemName;
            try
            {
                if (sItemName.IndexOf(' ') >= 0)
                {
                    // 折分物品名称(信件物品的名称后面加了使用次数)
                    HUtil32.GetValidStr3(sItemName, ref sItemName, new char[] { ' ' });
                }
                bo11 = false;
                if (!m_boSellOffOK)
                {
                    for (var i = this.m_ItemList.Count - 1; i >= 0; i--)
                    {
                        if (this.m_ItemList.Count <= 0)
                        {
                            break;
                        }
                        UserItem = this.m_ItemList[i];
                        if (UserItem == null)
                        {
                            continue;
                        }
                        if (UserItem.MakeIndex == nItemIdx)
                        {
                            // 取自定义物品名称
                            sUserItemName = "";
                            if (UserItem.btValue[13] == 1)
                            {
                                sUserItemName = M2Share.ItemUnit.GetCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                            }
                            if (sUserItemName == "")
                            {
                                sUserItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                            }
                            if (sUserItemName.ToLower().CompareTo(sItemName.ToLower()) == 0 && m_SellOffItemList.Count < 9)
                            {
                                //if (this.CheckItemValue(UserItem, 1))
                                //{
                                //    break;
                                //}
                                //else if (this.PlugOfCheckCanItem(1, sUserItemName, false, 0, 0)) // 禁止物品规则(管理插件功能)
                                //{
                                //    break;
                                //}
                                m_SellOffItemList.Add(UserItem);
                                this.SendMsg(this, Grobal2.RM_SELLOFFADDITEM_OK, 0, 0, 0, 0, ""); // 放物品成功
                                this.m_ItemList.RemoveAt(i);
                                //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品
                                bo11 = true;
                                break;
                            }
                        }
                    }
                }
                if (!bo11)
                {
                    this.SendMsg(this, Grobal2.RM_SellOffADDITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            finally
            {

            }
        }

        // 客户端删除出售物品窗里的物品
        private void ClientDelSellOffItem(int nItemIdx, string sItemName)
        {
            bool bo11;
            TUserItem UserItem;
            string sUserItemName;
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new char[] { ' ' });
            }
            bo11 = false;
            if (!m_boSellOffOK)
            {
                for (var i = m_SellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (m_SellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    UserItem = m_SellOffItemList[i];
                    if (UserItem == null)
                    {
                        continue;
                    }
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        // 取自定义物品名称
                        sUserItemName = "";
                        if (UserItem.btValue[13] == 1)
                        {
                            sUserItemName = M2Share.ItemUnit.GetCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                        }
                        if (sUserItemName == "")
                        {
                            sUserItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                        }
                        if (sUserItemName.ToLower().CompareTo(sItemName.ToLower()) == 0)
                        {
                            //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品 
                            this.m_ItemList.Add(UserItem);
                            this.SendMsg(this, Grobal2.RM_SELLOFFDELITEM_OK, 0, 0, 0, 0, "");
                            m_SellOffItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                this.SendMsg(this, Grobal2.RM_SELLOFFDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        /// <summary>
        /// 出售人取消正在出售中的交易
        /// </summary>
        private void ClientCancelSellOffIng()
        {
            TDealOffInfo DealOffInfo;
            MirItem StdItem;
            TUserItem UserItem;
            byte nCode;
            bool boot;
            nCode = 0;
            try
            {
                if (M2Share.sSellOffItemList == null || M2Share.sSellOffItemList.Count == 0 || !IsEnoughBag())
                {
                    return;
                }
                for (var i = M2Share.sSellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.sSellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    nCode = 1;
                    DealOffInfo = M2Share.sSellOffItemList[i];
                    nCode = 2;
                    if (DealOffInfo != null)
                    {
                        nCode = 12;
                        if (DealOffInfo.sDealCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && (DealOffInfo.N == 0 || DealOffInfo.N == 3))
                        {
                            DealOffInfo.N = 4;
                            nCode = 3;
                            for (var j = 0; j <= 9; j++)
                            {
                                nCode = 4;
                                StdItem = M2Share.UserEngine.GetStdItem(DealOffInfo.UseItems[j].wIndex);
                                if (StdItem != null)
                                {
                                    nCode = 5;
                                    //UserItem = new TUserItem();
                                    //FillChar(UserItem, sizeof(TUserItem), '\0');
                                    // FillChar(UserItem^.btValue, SizeOf(UserItem^.btValue), 0);//20080820 增加
                                    UserItem = DealOffInfo.UseItems[j];
                                    nCode = 6;
                                    if (IsEnoughBag())// 人物的包裹是否满了
                                    {
                                        if (this.IsAddWeightAvailable(StdItem.Weight))// 检查负重
                                        {
                                            nCode = 7;
                                            if (this.AddItemToBag(UserItem))
                                            {
                                                SendAddItem(UserItem);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.DropItemDown(UserItem, 3, false, this, this);
                                    }
                                }
                                // 是金刚石
                                else if (DealOffInfo.UseItems[j].MakeIndex > 0 && DealOffInfo.UseItems[j].wIndex == Int16.MaxValue && DealOffInfo.UseItems[j].Dura == Int16.MaxValue && DealOffInfo.UseItems[j].DuraMax == Int16.MaxValue)
                                {
                                    nCode = 8;
                                    m_nGold += DealOffInfo.UseItems[j].MakeIndex; // 增加金刚石
                                    nCode = 9;
                                    this.GameGoldChanged(); // 更新金刚石数量
                                }
                            }
                            nCode = 10;
                            M2Share.sSellOffItemList.RemoveAt(i);
                            Dispose(DealOffInfo);
                            nCode = 11;
                            this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "取消寄售成功！");
                            // FrmDB.SaveSellOffItemList;//保存元宝寄售列表
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TPlayObject.ClientCancelSellOffIng Code:" + nCode.ToString());
            }
        }

        /// <summary>
        /// 购买人取消交易
        /// </summary>
        /// <param name="DealCharName"></param>
        private void ClientBuyCancelSellOff(string DealCharName)
        {
            TDealOffInfo DealOffInfo;
            for (var I = M2Share.sSellOffItemList.Count - 1; I >= 0; I--)
            {
                if (M2Share.sSellOffItemList.Count <= 0)
                {
                    break;
                }
                DealOffInfo = M2Share.sSellOffItemList[I];
                if (DealOffInfo != null)
                {
                    if (DealOffInfo.sDealCharName.ToLower().CompareTo(DealCharName.ToLower()) == 0 && DealOffInfo.sBuyCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && DealOffInfo.N == 0)
                    {
                        DealOffInfo.N = 3;// 购买人取消标识
                        // sSellOffItemList.Delete(I);//20081022
                        // sSellOffItemList.Add(DealOffInfo);//20081022
                        this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "取消交易成功！");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 购买寄售物品
        /// </summary>
        /// <param name="DealCharName"></param>
        private void ClientBuySellOffItme(string DealCharName)
        {
            TDealOffInfo DealOffInfo;
            MirItem StdItem;
            TUserItem UserItem;
            TPlayObject PlayObject;
            byte nCode;
            bool boot;
            nCode = 0;
            try
            {
                for (var i = M2Share.sSellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.sSellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    nCode = 1;
                    DealOffInfo = M2Share.sSellOffItemList[i];
                    if (DealOffInfo != null)
                    {
                        if (DealOffInfo.sDealCharName.ToLower().CompareTo(DealCharName.ToLower()) == 0 && DealOffInfo.sBuyCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && DealOffInfo.N == 0)
                        {
                            nCode = 2;
                            DealOffInfo.N = 4;
                            if (m_nGameGold >= DealOffInfo.nSellGold + M2Share.g_Config.nDecUserGameGold)// 每次扣多少元宝(元宝寄售)
                            {
                                m_nGameGold -= DealOffInfo.nSellGold + M2Share.g_Config.nDecUserGameGold; // 扣出元宝
                                if (m_nGameGold < 0)
                                {
                                    m_nGameGold = 0;
                                }
                                this.GameGoldChanged(); // 更新元宝数量
                                nCode = 3; // 给出售人元宝
                                PlayObject = M2Share.UserEngine.GetPlayObject(DealOffInfo.sDealCharName);
                                if (PlayObject == null)// 出售人不在线
                                {
                                    DealOffInfo.N = 1; // 物品已出售,出售人未得到元宝
                                    // sSellOffItemList.Delete(I);
                                    // sSellOffItemList.Add(DealOffInfo);
                                }
                                else
                                {
                                    if (PlayObject.m_boOffLineFlag)  // 挂机
                                    {
                                        DealOffInfo.N = 1; // 物品已出售,出售人未得到元宝
                                    }
                                    else
                                    {
                                        nCode = 10;
                                        UpdateSellOffInfo(1);
                                        DealOffInfo.N = 2; // 交易结束
                                        PlayObject.m_nGameGold += DealOffInfo.nSellGold;
                                        PlayObject.GameGoldChanged();
                                        nCode = 7;
                                        PlayObject.SysMsg(string.Format(M2Share.sGetSellOffGlod, new object[] { DealOffInfo.nSellGold, M2Share.g_Config.sGameGoldName }), TMsgColor.c_Red, TMsgType.t_Hint);
                                        nCode = 8;
                                        if (M2Share.g_boGameLogGameGold)
                                        {
                                            M2Share.AddGameDataLog(string.Format(M2Share.g_sGameLogMsg1, new object[] { Grobal2.LOG_GAMEGOLD, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGameGoldName, PlayObject.m_nGameGold, "寄售获得(" + DealOffInfo.nSellGold.ToString() + ')', this.m_sCharName }));
                                        }
                                    }
                                }
                                // FrmDB.SaveSellOffItemList;//保存元宝寄售列表 20080317
                                for (var j = 0; j <= 9; j++)
                                {
                                    StdItem = M2Share.UserEngine.GetStdItem(DealOffInfo.UseItems[j].wIndex);
                                    if (StdItem != null)
                                    {
                                        nCode = 12;
                                        //UserItem = new TUserItem();
                                        //FillChar(UserItem, sizeof(TUserItem), '\0');
                                        UserItem = DealOffInfo.UseItems[j];
                                        nCode = 13;
                                        if (IsEnoughBag()) // 检查人物的包裹是否满了 
                                        {
                                            //ClearCopyItem(0, UserItem.wIndex, UserItem.MakeIndex); // 清理包裹和仓库复制物品 
                                            if (this.AddItemToBag(UserItem))
                                            {
                                                nCode = 14;
                                                SendAddItem(UserItem);
                                                if (StdItem.NeedIdentify == 1)
                                                {
                                                   // M2Share.AddGameDataLog('9' + "\09" + this.m_sMapName + "(*)" + "\09" + this.m_nCurrX.ToString() + "\09" + this.m_nCurrY.ToString() + "\09" + this.m_sCharName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex.ToString() + "\09" + '(' + HUtil32.LoWord(StdItem.DC).ToString() + '/' + HUtil32.HiWord(StdItem.DC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.MC).ToString() + '/' + HUtil32.HiWord(StdItem.MC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.SC).ToString() + '/' + HUtil32.HiWord(StdItem.SC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.AC).ToString() + '/' + HUtil32.HiWord(StdItem.AC).ToString() + ')' + '(' + HUtil32.LoWord(StdItem.MAC).ToString() + '/' + HUtil32.HiWord(StdItem.MAC).ToString() + ')' + UserItem.btValue[0].ToString() + '/' + UserItem.btValue[1].ToString() + '/' + UserItem.btValue[2].ToString() + '/' + UserItem.btValue[3].ToString() + '/' + UserItem.btValue[4].ToString() + '/' + UserItem.btValue[5].ToString() + '/' + UserItem.btValue[6].ToString() + '/' + UserItem.btValue[7].ToString() + '/' + UserItem.btValue[8].ToString() + '/' + UserItem.btValue[14].ToString() + "\09" + DealOffInfo.sDealCharName);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.DropItemDown(UserItem, 3, false, this, this);
                                        }
                                    }
                                    // 是金刚石
                                    else if (DealOffInfo.UseItems[j].MakeIndex > 0 && DealOffInfo.UseItems[j].wIndex == Int16.MaxValue && DealOffInfo.UseItems[j].Dura == Int16.MaxValue && DealOffInfo.UseItems[j].DuraMax == Int16.MaxValue)
                                    {
                                        nCode = 15;
                                        m_nGold += DealOffInfo.UseItems[j].MakeIndex; // 增加金刚石
                                        this.SysMsg(DealOffInfo.UseItems[j].MakeIndex.ToString() + " 颗金刚石增加", TMsgColor.c_Blue, TMsgType.t_Hint);
                                    }
                                }
                                nCode = 16;
                                this.SendMsg(this, Grobal2.RM_SELLOFFBUY_OK, 0, 0, 0, 0, "");// 购买成功
                                this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "[成功] 系统已经成功接受您的申请");
                                break;
                            }
                            else
                            {
                                DealOffInfo.N = 0;
                                this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "[错误] 您的申请提交不成功");
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TPlayObject.ClientBuySellOffItme Code:" + nCode.ToString());
            }
        }

        // 人物上线,检查是否有交易结束还没得到元宝 
        private void GetSellOffGlod()
        {
            TDealOffInfo DealOffInfo;
            //if (m_boNotOnlineAddExp) // 挂机则退出 
            //{
            //    return;
            //}
            try
            {
                for (var i = M2Share.sSellOffItemList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.sSellOffItemList.Count <= 0)
                    {
                        break;
                    }
                    DealOffInfo = M2Share.sSellOffItemList[i];
                    if (DealOffInfo != null)
                    {
                        if (DealOffInfo.sDealCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && DealOffInfo.N == 1)
                        {
                            UpdateSellOffInfo(0);
                            DealOffInfo.N = 2; // 交易结束
                            // sSellOffItemList.Delete(I);
                            // sSellOffItemList.Add(DealOffInfo);
                            m_nGameGold += DealOffInfo.nSellGold;
                            this.GameGoldChanged();
                            this.SysMsg(string.Format(M2Share.sGetSellOffGlod, new object[] { DealOffInfo.nSellGold, M2Share.g_Config.sGameGoldName }), TMsgColor.c_Red, TMsgType.t_Hint);
                            if (M2Share.g_boGameLogGameGold)
                            {
                                M2Share.AddGameDataLog(string.Format(M2Share.g_sGameLogMsg1, new object[] { Grobal2.LOG_GAMEGOLD, this.m_sMapName, this.m_nCurrX, this.m_nCurrY,
                                        this.m_sCharName, M2Share.g_Config.sGameGoldName, m_nGameGold, "寄售获得(" + DealOffInfo.nSellGold.ToString() + ')', DealOffInfo.sBuyCharName }));
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TPlayObject.GetSellOffGlod");
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
            TDealOffInfo DealOffInfo;
            var result = "您未开通" + M2Share.g_Config.sGameGoldName + "寄售服务,请先开通！！！\\ \\<返回/@main>";
            if (bo_YBDEAL)
            {
                // 已开通元宝服务
                if (M2Share.sSellOffItemList.Count > 0)
                {
                    for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                    {
                        DealOffInfo = M2Share.sSellOffItemList[i];
                        if (DealOffInfo != null)
                        {
                            if (DealOffInfo.sDealCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && DealOffInfo.N == 2)
                            {
                                result = "最后一笔出售记录:\\" + string.Format("   yyyy年mm月dd日 hh时nn分", DealOffInfo.dSellDateTime) + ",\\  您与" + DealOffInfo.sBuyCharName + "交易成功,获得了" + DealOffInfo.nSellGold.ToString() + '个' + M2Share.g_Config.sGameGoldName + "。\\ \\<返回/@main>";
                                return result;
                            }
                            else if (DealOffInfo.sBuyCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && (DealOffInfo.N == 1 || DealOffInfo.N == 2))
                            {
                                result = "最后一笔购买记录:\\" + string.Format("   yyyy年mm月dd日 hh时nn分", DealOffInfo.dSellDateTime) + ",\\  您与" + DealOffInfo.sDealCharName + "交易成功,支付了" + DealOffInfo.nSellGold.ToString() + '个' + M2Share.g_Config.sGameGoldName + "。\\ \\<返回/@main>";
                                return result;
                            }
                        }
                    }
                }
                result = "您未进行任何寄售交易！！！\\ \\<返回/@main>";
            }
            return result;
        }

        /// <summary>
        /// 查询玩家是否操作过寄售
        /// </summary>
        /// <param name="nCode"></param>
        /// <returns></returns>
        public bool SellOffInTime(int nCode)
        {
            TDealOffInfo DealOffInfo;
            var result = false;
            if (M2Share.sSellOffItemList.Count > 0)
            {
                for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                {
                    DealOffInfo = M2Share.sSellOffItemList[i];
                    if (DealOffInfo != null)
                    {
                        switch (nCode)
                        {
                            case 0: // 出售者
                                if (DealOffInfo.sDealCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && (DealOffInfo.N == 0 || DealOffInfo.N == 3))
                                {
                                    result = true;
                                    break;
                                }
                                break;
                            case 1: // 购买者
                                if (DealOffInfo.sBuyCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) == 0 && DealOffInfo.N == 0)
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
        /// <param name="sBuyCharName"></param>
        /// <param name="nSellGold"></param>
        /// <param name="nGameDiamond"></param>
        /// <param name="nCode"></param>
        private void ClientSellOffEnd(string sBuyCharName, int nSellGold, int nGameDiamond, int nCode)
        {
            bool bo11;
            TUserItem UserItem;
            MirItem StdItem;
            TDealOffInfo DealOffInfo;
            m_boSellOffOK = true;
            bo11 = false;
            if (m_boSellOffOK && (m_SellOffItemList.Count > 0 || nGameDiamond > 0) && m_SellOffItemList.Count < 10 && sBuyCharName.Length < 20 && nSellGold > 0 && nSellGold < 100000000 
                && sBuyCharName.ToLower().CompareTo(this.m_sCharName.ToLower()) != 0)
            {
                // 不能自己寄售给自己
                DealOffInfo = new TDealOffInfo();
                for (var j = 0; j <= 9; j++)
                {
                    TUserItem _wvar1 = DealOffInfo.UseItems[j];
                    _wvar1.MakeIndex = 0;
                    _wvar1.wIndex = 0;
                    _wvar1.Dura = 0;
                    _wvar1.DuraMax = 0;
                    _wvar1.btValue[0] = 0;
                    _wvar1.btValue[1] = 0;
                    _wvar1.btValue[2] = 0;
                    _wvar1.btValue[3] = 0;
                    _wvar1.btValue[4] = 0;
                    _wvar1.btValue[5] = 0;
                    _wvar1.btValue[6] = 0;
                    _wvar1.btValue[7] = 0;
                    _wvar1.btValue[8] = 0;
                    _wvar1.btValue[9] = 0;
                    _wvar1.btValue[10] = 0;
                    _wvar1.btValue[11] = 0;
                    _wvar1.btValue[12] = 0;
                    _wvar1.btValue[13] = 0;
                    _wvar1.btValue[14] = 0;
                    _wvar1.btValue[15] = 0;
                    _wvar1.btValue[16] = 0;
                    _wvar1.btValue[17] = 0;
                    _wvar1.btValue[18] = 0;
                    _wvar1.btValue[19] = 0;
                    _wvar1.btValue[20] = 0;
                }
                if (m_SellOffItemList.Count > 0)
                {
                    for (var i = 0; i < m_SellOffItemList.Count; i++)
                    {
                        UserItem = m_SellOffItemList[i];
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (UserItem != null && UserItem.MakeIndex > 0)
                        {
                            DealOffInfo.UseItems[i] = UserItem;
                        }
                    }
                }
                for (var j = 0; j <= 9; j++)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(DealOffInfo.UseItems[j].wIndex);
                    if (StdItem == null && nGameDiamond > 0 && nGameDiamond < 10000 && nCode == Int16.MaxValue)// 物品是金刚石
                    {
                        if (nGameDiamond > m_nGold) // 金刚石数量大于玩家的数量时则反回失败
                        {
                            this.SendMsg(this, Grobal2.RM_SELLOFFEND_FAIL, 0, 0, 0, 0, "");
                            this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "[错误] 你没有那么多金币");
                            GetBackSellOffItems(); // 返回物品
                            return;
                        }
                        m_nGold -= nGameDiamond;
                        this.GameGoldChanged(); // 更新金刚石数量
                        DealOffInfo.UseItems[j].MakeIndex = nGameDiamond; // 金刚石数量
                        DealOffInfo.UseItems[j].wIndex = UInt16.MaxValue;
                        DealOffInfo.UseItems[j].Dura = UInt16.MaxValue;
                        DealOffInfo.UseItems[j].DuraMax = UInt16.MaxValue;
                        break;
                    }
                }
                DealOffInfo.sDealCharName = this.m_sCharName; // 寄售人
                DealOffInfo.sBuyCharName = sBuyCharName.Trim(); // 购买人
                DealOffInfo.nSellGold = nSellGold; // 元宝数
                DealOffInfo.dSellDateTime = DateTime.Now; // 操作时间
                DealOffInfo.N = 0; // 标识
                M2Share.sSellOffItemList.Add(DealOffInfo); // 增加到元宝寄售列表中
                this.SendMsg(this, Grobal2.RM_SELLOFFEND_OK, 0, 0, 0, 0, "");
                m_nGameGold -= M2Share.g_Config.nDecUserGameGold; // 每次扣多少元宝(元宝寄售) 
                if (m_nGameGold < 0)
                {
                    m_nGameGold = 0;
                }
                this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "[成功] 系统已经成功接受您的申请");
                bo11 = true;
                // FrmDB.SaveSellOffItemList;//保存元宝寄售列表 
                m_boSellOffOK = false;
                m_SellOffItemList.Clear();
            }
            if (!bo11)
            {
                // 失败则返回物品给玩家
                this.SendMsg(this, Grobal2.RM_SELLOFFEND_FAIL, 0, 0, 0, 0, "");
                this.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "[错误:] 寄售物品失败");
                GetBackSellOffItems();
            }
        }

        /// <summary>
        /// 元宝寄售取消出售
        /// </summary>
        public void SellOffCancel()
        {
            this.SendMsg(this, Grobal2.RM_SELLOFFCANCEL, 0, 0, 0, 0, "");
            GetBackSellOffItems();
        }

        /// <summary>
        /// 取备份元宝寄售列表物品
        /// </summary>
        public void GetBackSellOffItems()
        {
            if (m_SellOffItemList.Count > 0)
            {
                for (var I = m_SellOffItemList.Count - 1; I >= 0; I--)
                {
                    this.m_ItemList.Add(m_SellOffItemList[I]);
                    m_SellOffItemList.RemoveAt(I);
                }
            }
            m_boSellOffOK = false;// 确认元宝寄售标志 
        }
    }
}
