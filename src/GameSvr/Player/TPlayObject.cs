using GameSvr.Actor;
using GameSvr.Castle;
using GameSvr.Command;
using GameSvr.Event.Events;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Npc;
using GameSvr.Services;
using System.Collections;
using System.Text.RegularExpressions;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Player
{
    public partial class TPlayObject : AnimalObject
    {
        private bool ClientPickUpItem_IsSelf(int BaseObject)
        {
            bool result;
            if (BaseObject == 0 || this.ObjectId == BaseObject)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ClientPickUpItem_IsOfGroup(int BaseObject)
        {
            if (m_GroupOwner == null)
            {
                return false;
            }
            for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
            {
                if (m_GroupOwner.m_GroupMembers[i].ObjectId == BaseObject)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ClientPickUpItem()
        {
            var result = false;
            if (m_boDealing)
            {
                return false;
            }
            var mapItem = m_PEnvir.GetItem(m_nCurrX, m_nCurrY);
            if (mapItem == null)
            {
                return false;
            }
            if ((HUtil32.GetTickCount() - mapItem.CanPickUpTick) > M2Share.g_Config.dwFloorItemCanPickUpTime)// 2 * 60 * 1000
            {
                mapItem.OfBaseObject = 0;
            }
            if (!ClientPickUpItem_IsSelf(mapItem.OfBaseObject) && !ClientPickUpItem_IsOfGroup(mapItem.OfBaseObject))
            {
                SysMsg(M2Share.g_sCanotPickUpItem, MsgColor.Red, MsgType.Hint);
                return false;
            }
            if (mapItem.Name.Equals(Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase))
            {
                if (m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, CellType.ItemObject, mapItem) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(Grobal2.RM_ITEMHIDE, 0, mapItem.ObjectId, m_nCurrX, m_nCurrY, "");
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('4' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME
                                                   + "\t" + mapItem.Count + "\t" + '1' + "\t" + '0');
                        }
                        GoldChanged();
                        Dispose(mapItem);
                    }
                    else
                    {
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, CellType.ItemObject, mapItem);
                    }
                }
                return result;
            }
            if (IsEnoughBag())
            {
                if (m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, CellType.ItemObject, mapItem) == 1)
                {
                    var UserItem = mapItem.UserItem;
                    var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, mapItem.ObjectId, m_nCurrX, m_nCurrY, "");
                        AddItemToBag(UserItem);
                        if (!M2Share.IsCheapStuff(StdItem.StdMode))
                        {
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('4' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name
                                                       + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        Dispose(mapItem);
                        if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                        {
                            this.SendAddItem(UserItem);
                        }
                        result = true;
                    }
                    else
                    {
                        Dispose(UserItem);
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, CellType.ItemObject, mapItem);
                    }
                }
            }
            return result;
        }

        private void WinExp(int dwExp)
        {
            if (m_Abil.Level > M2Share.g_Config.nLimitExpLevel)
            {
                dwExp = M2Share.g_Config.nLimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = M2Share.g_Config.dwKillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = m_nKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(m_nKillMonExpRate / 100 * dwExp);// 人物指定的杀怪经验倍数
                if (m_PEnvir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(m_PEnvir.Flag.nEXPRATE / 100 * dwExp);// 地图上指定杀怪经验倍数
                }
                if (m_boExpItem) // 物品经验倍数
                {
                    dwExp = HUtil32.Round(m_rExpItem * dwExp);
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            m_Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, Grobal2.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (m_Abil.Exp >= m_Abil.MaxExp)
            {
                m_Abil.Exp -= m_Abil.MaxExp;
                if (m_Abil.Level < M2Share.MAXUPLEVEL)
                {
                    m_Abil.Level++;
                }
                HasLevelUp(m_Abil.Level - 1);
                AddBodyLuck(100);
                M2Share.AddGameDataLog("12" + "\t" + m_sMapName + "\t" + m_Abil.Level + "\t" + m_Abil.Exp + "\t" + m_sCharName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public bool IncGold(int tGold)
        {
            var result = false;
            if (m_nGold + tGold <= M2Share.g_Config.nHumanMaxGold)
            {
                m_nGold += tGold;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查包裹是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsEnoughBag()
        {
            return m_ItemList.Count < Grobal2.MAXBAGITEM;
        }

        /// <summary>
        /// 是否超过包裹最大负重
        /// </summary>
        /// <param name="nWeight"></param>
        /// <returns></returns>
        public bool IsAddWeightAvailable(int nWeight)
        {
            return m_WAbil.Weight + nWeight <= m_WAbil.MaxWeight;
        }

        public void SendAddItem(TUserItem UserItem)
        {
            StdItem Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (Item == null)
            {
                return;
            }
            TClientItem ClientItem = new TClientItem();
            Item.GetStandardItem(ref ClientItem.Item);
            Item.GetItemAddValue(UserItem, ref ClientItem.Item);
            ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
            ClientItem.MakeIndex = UserItem.MakeIndex;
            ClientItem.Dura = UserItem.Dura;
            ClientItem.DuraMax = UserItem.DuraMax;
            var StdItem = ClientItem.Item;
            if (StdItem.StdMode == 50)
            {
                ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
            }
            if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
            {
                if (UserItem.btValue[8] == 0)
                {
                    ClientItem.Item.Shape = 0;
                }
                else
                {
                    ClientItem.Item.Shape = 130;
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDITEM, ObjectId, 0, 0, 1);
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
        }

        internal bool IsBlockWhisper(string sName)
        {
            var result = false;
            for (var i = 0; i < this.m_BlockWhisperList.Count; i++)
            {
                if (string.Compare(sName, this.m_BlockWhisperList[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void SendSocket(string sMsg)
        {
            if (m_boOffLineFlag)
            {
                return;
            }
            var MsgHdr = new PacketHeader
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = m_nSocket,
                SessionId = m_nGSocketIdx,
                Ident = Grobal2.GM_DATA
            };
            if (!string.IsNullOrEmpty(sMsg))
            {
                var bMsg = HUtil32.GetBytes(sMsg);
                MsgHdr.PackLength = -(bMsg.Length);
                var nSendBytes = Math.Abs(MsgHdr.PackLength) + 20;
                using var memoryStream = new MemoryStream();
                using var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(nSendBytes);
                backingStream.Write(MsgHdr.GetBuffer());
                if (bMsg.Length > 0)
                {
                    backingStream.Write(bMsg);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var data = new byte[memoryStream.Length];
                memoryStream.Read(data, 0, data.Length);
                M2Share.GateManager.AddGateBuffer(m_nGateIdx, data);
            }
        }

        private void SendSocket(ClientPacket DefMsg)
        {
            SendSocket(DefMsg, "");
        }

        internal virtual void SendSocket(ClientPacket defMsg, string sMsg)
        {
            if (m_boOffLineFlag && defMsg.Ident != Grobal2.SM_OUTOFCONNECTION)
            {
                return;
            }
            var messageHead = new PacketHeader
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = m_nSocket,
                SessionId = m_nGSocketIdx,
                Ident = Grobal2.GM_DATA
            };
            var nSendBytes = 0;
            using var memoryStream = new MemoryStream();
            using var backingStream = new BinaryWriter(memoryStream);
            byte[] bMsg = null;
            if (defMsg != null)
            {
                bMsg = HUtil32.GetBytes(sMsg);
                if (!string.IsNullOrEmpty(sMsg))
                {
                    messageHead.PackLength = bMsg.Length + 12;
                }
                else
                {
                    messageHead.PackLength = 12;
                }
                nSendBytes = messageHead.PackLength + PacketHeader.PacketSize;
                backingStream.Write(nSendBytes);
                backingStream.Write(messageHead.GetBuffer());
                backingStream.Write(defMsg.GetBuffer());
            }
            else if (!string.IsNullOrEmpty(sMsg))
            {
                bMsg = HUtil32.GetBytes(sMsg);
                messageHead.PackLength = -(bMsg.Length);
                nSendBytes = Math.Abs(messageHead.PackLength) + PacketHeader.PacketSize;
                backingStream.Write(nSendBytes);
                backingStream.Write(messageHead.GetBuffer());
            }
            if (bMsg != null && bMsg.Length > 0)
            {
                backingStream.Write(bMsg);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            M2Share.GateManager.AddGateBuffer(m_nGateIdx, data);
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            m_DefMsg = Grobal2.MakeDefaultMsg(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(m_DefMsg, EDcode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(m_DefMsg);
            }
        }

        private byte DayBright()
        {
            byte result;
            if (m_PEnvir.Flag.boDarkness)
            {
                result = 1;
            }
            else if (m_btBright == 1)
            {
                result = 0;
            }
            else if (m_btBright == 3)
            {
                result = 1;
            }
            else
            {
                result = 2;
            }
            if (m_PEnvir.Flag.boDayLight)
            {
                result = 0;
            }
            return result;
        }

        private void RefUserState()
        {
            var n8 = 0;
            if (m_PEnvir.Flag.boFightZone)
            {
                n8 = n8 | 1;
            }
            if (m_PEnvir.Flag.boSAFE)
            {
                n8 = n8 | 2;
            }
            if (m_boInFreePKArea)
            {
                n8 = n8 | 4;
            }
            SendDefMessage(Grobal2.SM_AREASTATE, n8, 0, 0, 0, "");
        }

        public void RefMyStatus()
        {
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
        }

        /// <summary>
        /// 祈祷套装生效
        /// </summary>
        private void ProcessSpiritSuite()
        {
            StdItem StdItem;
            TUserItem UseItem;
            if (!M2Share.g_Config.boSpiritMutiny || !m_bopirit)
            {
                return;
            }
            m_bopirit = false;
            for (var i = 0; i < m_UseItems.Length; i++)
            {
                UseItem = m_UseItems[i];
                if (UseItem == null)
                {
                    continue;
                }
                if (UseItem.wIndex <= 0)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UseItem.wIndex);
                if (StdItem != null)
                {
                    if (StdItem.Shape == 126 || StdItem.Shape == 127 || StdItem.Shape == 128 || StdItem.Shape == 129)
                    {
                        SendDelItems(UseItem);
                        UseItem.wIndex = 0;
                    }
                }
            }
            RecalcAbilitys();
            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + M2Share.g_Config.dwSpiritMutinyTime;
            M2Share.UserEngine.SendBroadCastMsg("神之祈祷，天地震怒，尸横遍野...", MsgType.System);
            SysMsg("祈祷发出强烈的宇宙效应", MsgColor.Green, MsgType.Hint);
        }

        private void LogonTimcCost()
        {
            int n08;
            string sC;
            if (m_nPayMent == 2 || M2Share.g_Config.boTestServer)
            {
                n08 = (HUtil32.GetTickCount() - m_dwLogonTick) / 1000;
            }
            else
            {
                n08 = 0;
            }
            sC = m_sIPaddr + "\t" + m_sUserID + "\t" + m_sCharName + "\t" + n08 + "\t" + m_dLogonTime.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + m_nPayMode;
            M2Share.AddLogonCostLog(sC);
            if (m_nPayMode == 2)
            {
                IdSrvClient.Instance.SendLogonCostMsg(m_sUserID, n08 / 60);
            }
        }

        private bool RunTo(byte btDir, bool boFlag, int nDestX, int nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = m_nCurrX;
                int nOldY = m_nCurrY;
                Direction = btDir;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 2 && m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 2 && m_nCurrY < m_PEnvir.Height - 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (m_nCurrY < m_PEnvir.Height - 2 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (m_nCurrX > 1 && m_nCurrY < m_PEnvir.Height - 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (m_nCurrX > 1 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX -= 2;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (m_nCurrX > 1 && m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY -= 2;
                        }
                        break;
                }
                if (m_nCurrX != nOldX || m_nCurrY != nOldY)
                {
                    if (Walk(Grobal2.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        m_nCurrX = (short)nOldX;
                        m_nCurrY = (short)nOldY;
                        m_PEnvir.MoveToMovingObject(nOldX, nOldY, this, m_nCurrX, m_nCurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        private bool HorseRunTo(byte btDir, bool boFlag)
        {
            int n10;
            int n14;
            const string sExceptionMsg = "[Exception] TPlayObject::HorseRunTo";
            var result = false;
            try
            {
                n10 = m_nCurrX;
                n14 = m_nCurrY;
                Direction = btDir;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 3 && m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrX += 3;
                            m_nCurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 3 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY, true) > 0)
                        {
                            m_nCurrX += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (m_nCurrX < m_PEnvir.Width - 3 && m_nCurrY < m_PEnvir.Height - 3 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrX += 3;
                            m_nCurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (m_nCurrY < m_PEnvir.Height - 3 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (m_nCurrX > 2 && m_nCurrY < m_PEnvir.Height - 3 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrX -= 3;
                            m_nCurrY += 3;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (m_nCurrX > 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY, true) > 0)
                        {
                            m_nCurrX -= 3;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (m_nCurrX > 2 && m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrX -= 3;
                            m_nCurrY -= 3;
                        }
                        break;
                }
                if (m_nCurrX != n10 || m_nCurrY != n14)
                {
                    if (Walk(Grobal2.RM_HORSERUN))
                    {
                        result = true;
                    }
                    else
                    {
                        m_nCurrX = (short)n10;
                        m_nCurrY = (short)n14;
                        m_PEnvir.MoveToMovingObject(n10, n14, this, m_nCurrX, m_nCurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        protected void ThrustingOnOff(bool boSwitch)
        {
            m_boUseThrusting = boSwitch;
            if (m_boUseThrusting)
            {
                SysMsg(M2Share.sThrustingOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sThrustingOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void HalfMoonOnOff(bool boSwitch)
        {
            m_boUseHalfMoon = boSwitch;
            if (m_boUseHalfMoon)
            {
                SysMsg(M2Share.sHalfMoonOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sHalfMoonOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void RedHalfMoonOnOff(bool boSwitch)
        {
            m_boRedUseHalfMoon = boSwitch;
            if (m_boRedUseHalfMoon)
            {
                SysMsg(M2Share.sRedHalfMoonOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sRedHalfMoonOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void SkillCrsOnOff(bool boSwitch)
        {
            m_boCrsHitkill = boSwitch;
            if (m_boCrsHitkill)
            {
                SysMsg(M2Share.sCrsHitOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sCrsHitOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void SkillTwinOnOff(bool boSwitch)
        {
            m_boTwinHitSkill = boSwitch;
            if (m_boTwinHitSkill)
            {
                SysMsg(M2Share.sTwinHitOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sTwinHitOff, MsgColor.Green, MsgType.Hint);
            }
        }

        private void Skill43OnOff(bool boSwitch)
        {
            m_bo43kill = boSwitch;
            if (m_bo43kill)
            {
                SysMsg("开启破空剑", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg("关闭破空剑", MsgColor.Green, MsgType.Hint);
            }
        }

        private bool AllowFireHitSkill()
        {
            if ((HUtil32.GetTickCount() - m_dwLatestFireHitTick) > 10 * 1000)
            {
                m_dwLatestFireHitTick = HUtil32.GetTickCount();
                m_boFireHitSkill = true;
                SysMsg(M2Share.sFireSpiritsSummoned, MsgColor.Green, MsgType.Hint);
                return true;
            }
            SysMsg(M2Share.sFireSpiritsFail, MsgColor.Red, MsgType.Hint);
            return false;
        }

        private bool AllowTwinHitSkill()
        {
            m_dwLatestTwinHitTick = HUtil32.GetTickCount();
            m_boTwinHitSkill = true;
            SysMsg("twin hit skill charged", MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void ClientClickNPC(int npcId)
        {
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            if (m_boDeath || m_boGhost)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - m_dwClickNpcTime) > M2Share.g_Config.dwClickNpcTime)
            {
                m_dwClickNpcTime = HUtil32.GetTickCount();
                var normNpc = (NormNpc)M2Share.UserEngine.FindMerchant(npcId) ?? (NormNpc)M2Share.UserEngine.FindNPC(npcId);
                if (normNpc != null)
                {
                    if (normNpc.m_PEnvir == m_PEnvir && Math.Abs(normNpc.m_nCurrX - m_nCurrX) <= 15 && Math.Abs(normNpc.m_nCurrY - m_nCurrY) <= 15)
                    {
                        normNpc.Click(this);
                    }
                }
            }
        }

        private int GetRangeHumanCount()
        {
            return M2Share.UserEngine.GetMapOfRangeHumanCount(m_PEnvir, m_nCurrX, m_nCurrY, 10);
        }

        private void GetStartPoint()
        {
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == m_PEnvir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        m_sHomeMap = M2Share.StartPointList[i].m_sMapName;
                        m_nHomeX = M2Share.StartPointList[i].m_nCurrX;
                        m_nHomeY = M2Share.StartPointList[i].m_nCurrY;
                    }
                }
            }

            if (PKLevel() >= 2)
            {
                m_sHomeMap = M2Share.g_Config.sRedHomeMap;
                m_nHomeX = M2Share.g_Config.nRedHomeX;
                m_nHomeY = M2Share.g_Config.nRedHomeY;
            }
        }

        private void MobPlace(string sX, string sY, string sMonName, string sCount)
        {

        }

        private void DealCancel()
        {
            if (!m_boDealing)
            {
                return;
            }
            m_boDealing = false;
            SendDefMessage(Grobal2.SM_DEALCANCEL, 0, 0, 0, 0, "");
            if (m_DealCreat != null)
            {
                (m_DealCreat as TPlayObject).DealCancel();
            }
            m_DealCreat = null;
            GetBackDealItems();
            SysMsg(M2Share.g_sDealActionCancelMsg, MsgColor.Green, MsgType.Hint);
            m_DealLastTick = HUtil32.GetTickCount();
        }

        public void DealCancelA()
        {
            m_Abil.HP = m_WAbil.HP;
            DealCancel();
        }

        public bool DecGold(int nGold)
        {
            if (m_nGold >= nGold)
            {
                m_nGold -= nGold;
                return true;
            }
            return false;
        }

        public void GainExp(int dwExp)
        {
            int n;
            int sumlv;
            TPlayObject PlayObject;
            const string sExceptionMsg = "[Exception] TPlayObject::GainExp";
            double[] bonus = { 1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2 };
            try
            {
                if (m_GroupOwner != null)
                {
                    sumlv = 0;
                    n = 0;
                    for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
                    {
                        PlayObject = m_GroupOwner.m_GroupMembers[i];
                        if (!PlayObject.m_boDeath && m_PEnvir == PlayObject.m_PEnvir && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12 && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12)
                        {
                            sumlv = sumlv + PlayObject.m_Abil.Level;
                            n++;
                        }
                    }
                    if (sumlv > 0 && n > 1)
                    {
                        if (n >= 0 && n <= Grobal2.GROUPMAX)
                        {
                            dwExp = HUtil32.Round(dwExp * bonus[n]);
                        }
                        for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
                        {
                            PlayObject = m_GroupOwner.m_GroupMembers[i];
                            if (!PlayObject.m_boDeath && m_PEnvir == PlayObject.m_PEnvir && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12 && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12)
                            {
                                if (M2Share.g_Config.boHighLevelKillMonFixExp)
                                {
                                    PlayObject.WinExp(HUtil32.Round(dwExp / n)); // 在高等级经验不变时，把组队的经验平均分配
                                }
                                else
                                {
                                    PlayObject.WinExp(HUtil32.Round(dwExp / sumlv * PlayObject.m_Abil.Level));
                                }
                            }
                        }
                    }
                    else
                    {
                        WinExp(dwExp);
                    }
                }
                else
                {
                    WinExp(dwExp);
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void GameTimeChanged()
        {
            if (m_btBright != M2Share.g_nGameTime)
            {
                m_btBright = (byte)M2Share.g_nGameTime;
                SendMsg(this, Grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
            }
        }

        public void GetBackDealItems()
        {
            if (m_DealItemList.Count > 0)
            {
                for (var i = 0; i < m_DealItemList.Count; i++)
                {
                    m_ItemList.Add(m_DealItemList[i]);
                }
            }
            m_DealItemList.Clear();
            m_nGold += m_nDealGolds;
            m_nDealGolds = 0;
            m_boDealOK = false;
        }

        public override string GeTBaseObjectInfo()
        {
            return this.m_sCharName + " 标识:" + this.ObjectId + " 权限等级: " + this.m_btPermission + " 管理模式: " + HUtil32.BoolToStr(this.m_boAdminMode)
                + " 隐身模式: " + HUtil32.BoolToStr(this.m_boObMode) + " 无敌模式: " + HUtil32.BoolToStr(this.m_boSuperMan) + " 地图:" + this.m_sMapName + '(' + this.m_PEnvir.MapDesc + ')'
                + " 座标:" + this.m_nCurrX + ':' + this.m_nCurrY + " 等级:" + this.m_Abil.Level + " 转生等级:" + m_btReLevel
                + " 经验:" + this.m_Abil.Exp + " 生命值: " + this.m_WAbil.HP + '-' + this.m_WAbil.MaxHP + " 魔法值: " + this.m_WAbil.MP + '-' + this.m_WAbil.MaxMP
                + " 攻击力: " + HUtil32.LoWord(this.m_WAbil.DC) + '-' + HUtil32.HiWord(this.m_WAbil.DC) + " 魔法力: " + HUtil32.LoWord(this.m_WAbil.MC) + '-'
                + HUtil32.HiWord(this.m_WAbil.MC) + " 道术: " + HUtil32.LoWord(this.m_WAbil.SC) + '-' + HUtil32.HiWord(this.m_WAbil.SC)
                + " 防御力: " + HUtil32.LoWord(this.m_WAbil.AC) + '-' + HUtil32.HiWord(this.m_WAbil.AC) + " 魔防力: " + HUtil32.LoWord(this.m_WAbil.MAC)
                + '-' + HUtil32.HiWord(this.m_WAbil.MAC) + " 准确:" + this.m_btHitPoint + " 敏捷:" + this.m_btSpeedPoint + " 速度:" + this.m_nHitSpeed
                + " 仓库密码:" + m_sStoragePwd + " 登录IP:" + m_sIPaddr + '(' + m_sIPLocal + ')' + " 登录帐号:" + m_sUserID + " 登录时间:" + m_dLogonTime
                + " 在线时长(分钟):" + ((HUtil32.GetTickCount() - m_dwLogonTick) / 60000) + " 登录模式:" + m_nPayMent + ' ' + M2Share.g_Config.sGameGoldName + ':' + m_nGameGold
                + ' ' + M2Share.g_Config.sGamePointName + ':' + m_nGamePoint + ' ' + M2Share.g_Config.sPayMentPointName + ':' + m_nPayMentPoint + " 会员类型:" + m_nMemberType
                + " 会员等级:" + m_nMemberLevel + " 经验倍数:" + (m_nKillMonExpRate / 100) + " 攻击倍数:" + (m_nPowerRate / 100) + " 声望值:" + m_btCreditPoint;
        }

        public int GetDigUpMsgCount()
        {
            var result = 0;
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == Grobal2.CM_BUTCH)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        public void GoldChange(string sChrName, int nGold)
        {
            string s10;
            string s14;
            if (nGold > 0)
            {
                s10 = "14";
                s14 = "增加完成";
            }
            else
            {
                s10 = "13";
                s14 = "以删减";
            }
            SysMsg(sChrName + " 的金币 " + nGold + " 金币" + s14, MsgColor.Green, MsgType.Hint);
            if (M2Share.g_boGameLogGold)
            {
                M2Share.AddGameDataLog(s10 + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + sChrName);
            }
        }

        public void ClearStatusTime()
        {
            this.m_wStatusTimeArr = new ushort[12];
        }

        private void SendMapDescription()
        {
            var nMUSICID = -1;
            if (m_PEnvir.Flag.boMUSIC)
            {
                nMUSICID = m_PEnvir.Flag.nMUSICID;
            }
            SendDefMessage(Grobal2.SM_MAPDESCRIPTION, nMUSICID, 0, 0, 0, m_PEnvir.MapDesc);
        }

        private void SendWhisperMsg(TPlayObject PlayObject)
        {
            if (PlayObject == this)
            {
                return;
            }
            if (PlayObject.m_btPermission >= 9 || m_btPermission >= 9)
            {
                return;
            }
            if (M2Share.UserEngine.PlayObjectCount < M2Share.g_Config.nSendWhisperPlayCount + M2Share.RandomNumber.Random(5))
            {
                return;
            }
        }

        private void ReadAllBook()
        {
            for (var i = 0; i < M2Share.UserEngine.m_MagicList.Count; i++)
            {
                var Magic = M2Share.UserEngine.m_MagicList[i];
                TUserMagic UserMagic = new TUserMagic
                {
                    MagicInfo = Magic,
                    wMagIdx = Magic.wMagicID,
                    btLevel = 2,
                    btKey = 0
                };
                UserMagic.btLevel = 0;
                UserMagic.nTranPoint = 100000;
                m_MagicList.Add(UserMagic);
                SendAddMagic(UserMagic);
            }
        }

        private void SendGoldInfo(bool boSendName)
        {
            var sMsg = string.Empty;
            if (m_nSoftVersionDateEx == 0)
            {
                return;
            }
            if (boSendName)
            {
                sMsg = M2Share.g_Config.sGameGoldName + '\r' + M2Share.g_Config.sGamePointName;
            }
            SendDefMessage(Grobal2.SM_GAMEGOLDNAME, m_nGameGold, HUtil32.LoWord(m_nGamePoint), HUtil32.HiWord(m_nGamePoint), 0, sMsg);
        }

        private void SendServerConfig()
        {
            if (m_nSoftVersionDateEx == 0)
            {
                return;
            }
            var nRunHuman = 0;
            var nRunMon = 0;
            var nRunNpc = 0;
            var nWarRunAll = 0;
            if (M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll)
            {
                nRunHuman = 1;
                nRunMon = 1;
                nRunNpc = 1;
                nWarRunAll = 1;
            }
            else
            {
                if (M2Share.g_Config.boRunHuman || m_PEnvir.Flag.boRUNHUMAN)
                {
                    nRunHuman = 1;
                }
                if (M2Share.g_Config.boRunMon || m_PEnvir.Flag.boRUNMON)
                {
                    nRunMon = 1;
                }
                if (M2Share.g_Config.boRunNpc)
                {
                    nRunNpc = 1;
                }
                if (M2Share.g_Config.boWarDisHumRun)
                {
                    nWarRunAll = 1;
                }
            }
            var ClientConf = M2Share.g_Config.ClientConf;
            ClientConf.boRunHuman = nRunHuman == 1;
            ClientConf.boRunMon = nRunMon == 1;
            ClientConf.boRunNpc = nRunNpc == 1;
            ClientConf.boWarRunAll = nWarRunAll == 1;
            ClientConf.wSpellTime = (ushort)(M2Share.g_Config.dwMagicHitIntervalTime + 300);
            ClientConf.wHitIime = (ushort)(M2Share.g_Config.dwHitIntervalTime + 500);
            var sMsg = EDcode.EncodeBuffer(ClientConf);
            var nRecog = HUtil32.MakeLong(HUtil32.MakeWord(nRunHuman, nRunMon), HUtil32.MakeWord(nRunNpc, nWarRunAll));
            short nParam = (short)HUtil32.MakeWord(5, 0);
            SendDefMessage(Grobal2.SM_SERVERCONFIG, nRecog, nParam, 0, 0, sMsg);
        }

        private void SendServerStatus()
        {
            if (m_btPermission < 10)
            {
                return;
            }
            //this.SysMsg((HUtil32.CalcFileCRC(Application.ExeName)).ToString(), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        // 检查角色的座标是否在指定误差范围以内
        // TargeTBaseObject 为要检查的角色，nX,nY 为比较的座标
        // 检查角色是否在指定座标的1x1 范围以内，如果在则返回True 否则返回 False
        protected bool CretInNearXY(TBaseObject TargeTBaseObject, int nX, int nY)
        {
            MapCellInfo cellInfo;
            CellObject OSObject;
            TBaseObject BaseObject;
            if (m_PEnvir == null)
            {
                M2Share.MainOutMessage("CretInNearXY nil PEnvir");
                return false;
            }
            for (var nCX = nX - 1; nCX <= nX + 1; nCX++)
            {
                for (var nCY = nY - 1; nCY <= nY + 1; nCY++)
                {
                    var cellsuccess = false;
                    cellInfo = m_PEnvir.GetCellInfo(nCX, nCY, ref cellsuccess);
                    if (cellsuccess && cellInfo.ObjList != null)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            OSObject = cellInfo.ObjList[i];
                            if (OSObject.CellType == CellType.MovingObject)
                            {
                                BaseObject = M2Share.ObjectManager.Get(OSObject.CellObjId);;
                                if (BaseObject != null)
                                {
                                    if (!BaseObject.m_boGhost && BaseObject == TargeTBaseObject)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        internal void SendUseitems()
        {
            string sSendMsg = string.Empty;
            for (var i = 0; i < m_UseItems.Length; i++)
            {
                if (m_UseItems[i] != null && m_UseItems[i].wIndex > 0)
                {
                    StdItem Item = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (Item != null)
                    {
                        TClientItem ClientItem = new TClientItem();
                        Item.GetStandardItem(ref ClientItem.Item);
                        Item.GetItemAddValue(m_UseItems[i], ref ClientItem.Item);
                        ClientItem.Item.Name = ItmUnit.GetItemName(m_UseItems[i]);
                        ClientItem.Dura = m_UseItems[i].Dura;
                        ClientItem.DuraMax = m_UseItems[i].DuraMax;
                        ClientItem.MakeIndex = m_UseItems[i].MakeIndex;
                        sSendMsg = sSendMsg + i + '/' + EDcode.EncodeBuffer(ClientItem) + '/';
                    }
                }
            }
            if (sSendMsg != "")
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSEITEMS, 0, 0, 0, 0);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private void SendUseMagic()
        {
            var sSendMsg = string.Empty;
            TUserMagic UserMagic;
            TClientMagic ClientMagic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                ClientMagic = new TClientMagic();
                ClientMagic.Key = (char)UserMagic.btKey;
                ClientMagic.Level = UserMagic.btLevel;
                ClientMagic.CurTrain = UserMagic.nTranPoint;
                ClientMagic.Def = UserMagic.MagicInfo;
                sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientMagic) + '/';
            }
            if (sSendMsg != "")
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDMYMAGIC, 0, 0, 0, (short)m_MagicList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private bool UseStdmodeFunItem(StdItem StdItem)
        {
            var result = false;
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@StdModeFunc" + StdItem.AniCount, false);
                result = true;
            }
            return result;
        }

        public void RecalcAdjusBonus_AdjustAb(byte abil, short val, ref short lov, ref short hiv)
        {
            var lo = HUtil32.LoByte(abil);
            var hi = HUtil32.HiByte(abil);
            lov = 0;
            hiv = 0;
            for (var i = 0; i < val; i++)
            {
                if (lo + 1 < hi)
                {
                    lo++;
                    lov++;
                }
                else
                {
                    hi++;
                    hiv++;
                }
            }
        }

        private void RecalcAdjusBonus()
        {
            TNakedAbility BonusTick;
            TNakedAbility NakedAbil;
            short adc;
            short amc;
            short asc;
            short aac;
            short amac;
            short ldc = 0;
            short lmc = 0;
            short lsc = 0;
            short lac = 0;
            short lmac = 0;
            short hdc = 0;
            short hmc = 0;
            short hsc = 0;
            short hac = 0;
            short hmac = 0;
            BonusTick = null;
            NakedAbil = null;
            switch (m_btJob)
            {
                case PlayJob.Warrior:
                    BonusTick = M2Share.g_Config.BonusAbilofWarr;
                    NakedAbil = M2Share.g_Config.NakedAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    BonusTick = M2Share.g_Config.BonusAbilofWizard;
                    NakedAbil = M2Share.g_Config.NakedAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    BonusTick = M2Share.g_Config.BonusAbilofTaos;
                    NakedAbil = M2Share.g_Config.NakedAbilofTaos;
                    break;
            }
            adc = (short)(m_BonusAbil.DC / BonusTick.DC);
            amc = (short)(m_BonusAbil.MC / BonusTick.MC);
            asc = (short)(m_BonusAbil.SC / BonusTick.SC);
            aac = (short)(m_BonusAbil.AC / BonusTick.AC);
            amac = (short)(m_BonusAbil.MAC / BonusTick.MAC);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.DC, adc, ref ldc, ref hdc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.MC, amc, ref lmc, ref hmc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.SC, asc, ref lsc, ref hsc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.AC, aac, ref lac, ref hac);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.MAC, amac, ref lmac, ref hmac);
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + ldc, HUtil32.HiWord(m_WAbil.DC) + hdc);
            m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC) + lmc, HUtil32.HiWord(m_WAbil.MC) + hmc);
            m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC) + lsc, HUtil32.HiWord(m_WAbil.SC) + hsc);
            m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.AC) + lac, HUtil32.HiWord(m_WAbil.AC) + hac);
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MAC) + lmac, HUtil32.HiWord(m_WAbil.MAC) + hmac);
            m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_BonusAbil.HP / BonusTick.HP);
            m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + m_BonusAbil.MP / BonusTick.MP);
        }

        private void ClientAdjustBonus(int nPoint, string sMsg)
        {
            var BonusAbil = new TNakedAbility();
            int nTotleUsePoint;
            //FillChar(BonusAbil, '\0');
            //EDcode.DecodeBuffer(sMsg, BonusAbil);
            nTotleUsePoint = BonusAbil.DC + BonusAbil.MC + BonusAbil.SC + BonusAbil.AC + BonusAbil.MAC + BonusAbil.HP + BonusAbil.MP + BonusAbil.Hit + BonusAbil.Speed + BonusAbil.X2;
            if (nPoint + nTotleUsePoint == m_nBonusPoint)
            {
                m_nBonusPoint = nPoint;
                m_BonusAbil.DC += BonusAbil.DC;
                m_BonusAbil.MC += BonusAbil.MC;
                m_BonusAbil.SC += BonusAbil.SC;
                m_BonusAbil.AC += BonusAbil.AC;
                m_BonusAbil.MAC += BonusAbil.MAC;
                m_BonusAbil.HP += BonusAbil.HP;
                m_BonusAbil.MP += BonusAbil.MP;
                m_BonusAbil.Hit += BonusAbil.Hit;
                m_BonusAbil.Speed += BonusAbil.Speed;
                m_BonusAbil.X2 += BonusAbil.X2;
                RecalcAbilitys();
                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            }
            else
            {
                SysMsg("非法数据调整!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        public int GetMyStatus()
        {
            var result = m_nHungerStatus / 1000;
            if (result > 4)
            {
                result = 4;
            }
            return result;
        }

        private void SendAdjustBonus()
        {
            var sSendMsg = string.Empty;
            switch (m_btJob)
            {
                case PlayJob.Warrior:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofWarr) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofWarr);
                    break;
                case PlayJob.Wizard:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofWizard) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofWizard);
                    break;
                case PlayJob.Taoist:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofTaos) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofTaos);
                    break;
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADJUST_BONUS, m_nBonusPoint, 0, 0, 0);
            SendSocket(m_DefMsg, sSendMsg);
        }

        private void ShowMapInfo(string sMap, string sX, string sY)
        {
            Envirnoment Map;
            var nX = (short)HUtil32.Str_ToInt(sX, 0);
            var nY = (short)HUtil32.Str_ToInt(sY, 0);
            if (sMap != "" && nX >= 0 && nY >= 0)
            {
                Map = M2Share.MapManager.FindMap(sMap);
                if (Map != null)
                {
                    var cellsuccess = false;
                    MapCellInfo cellInfo = Map.GetCellInfo(nX, nY, ref cellsuccess);
                    if (cellsuccess)
                    {
                        SysMsg("标志: " + cellInfo.Attribute, MsgColor.Green, MsgType.Hint);
                        if (cellInfo.ObjList != null)
                        {
                            SysMsg("对象数: " + cellInfo.Count, MsgColor.Green, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg("取地图单元信息失败: " + sMap, MsgColor.Red, MsgType.Hint);
                    }
                }
            }
            else
            {
                SysMsg("请按正确格式输入: " + M2Share.g_GameCommand.MAPINFO.sCmd + " 地图号 X Y", MsgColor.Green, MsgType.Hint);
            }
        }

        public void PKDie(TPlayObject PlayObject)
        {
            var nWinLevel = M2Share.g_Config.nKillHumanWinLevel;
            var nLostLevel = M2Share.g_Config.nKilledLostLevel;
            var nWinExp = M2Share.g_Config.nKillHumanWinExp;
            var nLostExp = M2Share.g_Config.nKillHumanLostExp;
            var boWinLEvel = M2Share.g_Config.boKillHumanWinLevel;
            var boLostLevel = M2Share.g_Config.boKilledLostLevel;
            var boWinExp = M2Share.g_Config.boKillHumanWinExp;
            var boLostExp = M2Share.g_Config.boKilledLostExp;
            if (m_PEnvir.Flag.boPKWINLEVEL)
            {
                boWinLEvel = true;
                nWinLevel = m_PEnvir.Flag.nPKWINLEVEL;
            }
            if (m_PEnvir.Flag.boPKLOSTLEVEL)
            {
                boLostLevel = true;
                nLostLevel = m_PEnvir.Flag.nPKLOSTLEVEL;
            }
            if (m_PEnvir.Flag.boPKWINEXP)
            {
                boWinExp = true;
                nWinExp = m_PEnvir.Flag.nPKWINEXP;
            }
            if (m_PEnvir.Flag.boPKLOSTEXP)
            {
                boLostExp = true;
                nLostExp = m_PEnvir.Flag.nPKLOSTEXP;
            }
            if (PlayObject.m_Abil.Level - m_Abil.Level > M2Share.g_Config.nHumanLevelDiffer)
            {
                if (!PlayObject.IsGoodKilling(this))
                {
                    PlayObject.IncPkPoint(M2Share.g_Config.nKillHumanAddPKPoint);
                    PlayObject.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                    SysMsg(format(M2Share.g_sYouKilledByMsg, m_LastHiter.m_sCharName), MsgColor.Red, MsgType.Hint);
                    PlayObject.AddBodyLuck(-M2Share.g_Config.nKillHumanDecLuckPoint);
                    if (PKLevel() < 1)
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            PlayObject.MakeWeaponUnlock();
                        }
                    }
                    if (M2Share.g_FunctionNPC != null)
                    {
                        M2Share.g_FunctionNPC.GotoLable(PlayObject, "@OnMurder", false);
                        M2Share.g_FunctionNPC.GotoLable(this, "@Murdered", false);
                    }
                }
                else
                {
                    PlayObject.SysMsg(M2Share.g_sYouProtectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                }
                return;
            }
            if (boWinLEvel)
            {
                if (PlayObject.m_Abil.Level + nWinLevel <= M2Share.MAXUPLEVEL)
                {
                    PlayObject.m_Abil.Level += (byte)nWinLevel;
                }
                else
                {
                    PlayObject.m_Abil.Level = M2Share.MAXUPLEVEL;
                }
                PlayObject.HasLevelUp(PlayObject.m_Abil.Level - nWinLevel);
                if (boLostLevel)
                {
                    if (PKLevel() >= 2)
                    {
                        if (m_Abil.Level >= nLostLevel * 2)
                        {
                            m_Abil.Level -= (byte)(nLostLevel * 2);
                        }
                    }
                    else
                    {
                        if (m_Abil.Level >= nLostLevel)
                        {
                            m_Abil.Level -= (byte)nLostLevel;
                        }
                    }
                }
            }
            if (boWinExp)
            {
                PlayObject.WinExp(nWinExp);
                if (boLostExp)
                {
                    if (m_Abil.Exp >= nLostExp)
                    {
                        if (m_Abil.Exp >= nLostExp)
                        {
                            m_Abil.Exp -= nLostExp;
                        }
                        else
                        {
                            m_Abil.Exp = 0;
                        }
                    }
                    else
                    {
                        if (m_Abil.Level >= 1)
                        {
                            m_Abil.Level -= 1;
                            m_Abil.Exp += GetLevelExp(m_Abil.Level);
                            if (m_Abil.Exp >= nLostExp)
                            {
                                m_Abil.Exp -= nLostExp;
                            }
                            else
                            {
                                m_Abil.Exp = 0;
                            }
                        }
                        else
                        {
                            m_Abil.Level = 0;
                            m_Abil.Exp = 0;
                        }
                    }
                }
            }
        }

        public bool CancelGroup()
        {
            var result = true;
            const string sCanceGrop = "你的小组被解散了.";
            if (m_GroupMembers.Count <= 1)
            {
                SendGroupText(sCanceGrop);
                m_GroupMembers.Clear();
                m_GroupOwner = null;
                result = false;
            }
            return result;
        }

        public void SendGroupMembers()
        {
            TPlayObject PlayObject;
            var sSendMsg = "";
            for (var i = 0; i < m_GroupMembers.Count; i++)
            {
                PlayObject = m_GroupMembers[i];
                sSendMsg = sSendMsg + PlayObject.m_sCharName + '/';
            }
            for (var i = 0; i < m_GroupMembers.Count; i++)
            {
                PlayObject = m_GroupMembers[i];
                PlayObject.SendDefMessage(Grobal2.SM_GROUPMEMBERS, 0, 0, 0, 0, sSendMsg);
            }
        }

        internal ushort GetSpellPoint(TUserMagic UserMagic)
        {
            return (ushort)(HUtil32.Round(UserMagic.MagicInfo.wSpell / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefSpell);
        }

        public bool DoMotaebo_CanMotaebo(TBaseObject BaseObject, int nMagicLevel)
        {
            var result = true;
            if (m_Abil.Level > BaseObject.m_Abil.Level && !BaseObject.m_boStickMode)
            {
                var nC = m_Abil.Level - BaseObject.m_Abil.Level;
                if (M2Share.RandomNumber.Random(20) < nMagicLevel * 4 + 6 + nC)
                {
                    if (IsProperTarget(BaseObject))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool DoMotaebo(byte nDir, int nMagicLevel)
        {
            int nDmg;
            TBaseObject BaseObject_30 = null;
            TBaseObject BaseObject_34 = null;
            short nX = 0;
            short nY = 0;
            var result = false;
            var bo35 = true;
            var n24 = nMagicLevel + 1;
            var n28 = n24;
            this.Direction = nDir;
            var PoseCreate = GetPoseCreate();
            if (PoseCreate != null)
            {
                for (var i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    PoseCreate = GetPoseCreate();
                    if (PoseCreate != null)
                    {
                        n28 = 0;
                        if (!DoMotaebo_CanMotaebo(PoseCreate, nMagicLevel))
                        {
                            break;
                        }
                        if (nMagicLevel >= 3)
                        {
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 2, ref nX, ref nY))
                            {
                                BaseObject_30 = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                                if (BaseObject_30 != null && DoMotaebo_CanMotaebo(BaseObject_30, nMagicLevel))
                                {
                                    BaseObject_30.CharPushed(Direction, 1);
                                }
                            }
                        }
                        BaseObject_34 = PoseCreate;
                        if (PoseCreate.CharPushed(Direction, 1) != 1)
                        {
                            break;
                        }
                        GetFrontPosition(ref nX, ref nY);
                        if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nX, nY, false) > 0)
                        {
                            m_nCurrX = nX;
                            m_nCurrY = nY;
                            SendRefMsg(Grobal2.RM_RUSH, nDir, m_nCurrX, m_nCurrY, 0, "");
                            bo35 = false;
                            result = true;
                        }
                        n24 -= 1;
                    }
                }
            }
            else
            {
                bo35 = false;
                for (var i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    GetFrontPosition(ref nX, ref nY);
                    if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nX, nY, false) > 0)
                    {
                        m_nCurrX = nX;
                        m_nCurrY = nY;
                        SendRefMsg(Grobal2.RM_RUSH, nDir, m_nCurrX, m_nCurrY, 0, "");
                        n28 -= 1;
                    }
                    else
                    {
                        if (m_PEnvir.CanWalk(nX, nY, true))
                        {
                            n28 = 0;
                        }
                        else
                        {
                            bo35 = true;
                            break;
                        }
                    }
                }
            }
            if (BaseObject_34 != null)
            {
                if (n24 < 0)
                {
                    n24 = 0;
                }
                nDmg = M2Share.RandomNumber.Random((n24 + 1) * 10) + (n24 + 1) * 10;
                nDmg = BaseObject_34.GetHitStruckDamage(this, nDmg);
                BaseObject_34.StruckDamage(nDmg);
                BaseObject_34.SendRefMsg(Grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.m_WAbil.HP, BaseObject_34.m_WAbil.MaxHP, ObjectId, "");
                if (BaseObject_34.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                {
                    BaseObject_34.SendMsg(BaseObject_34, Grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.m_WAbil.HP, BaseObject_34.m_WAbil.MaxHP, ObjectId, "");
                }
            }
            if (bo35)
            {
                GetFrontPosition(ref nX, ref nY);
                SendRefMsg(Grobal2.RM_RUSHKUNG, Direction, nX, nY, 0, "");
                SysMsg(M2Share.sMateDoTooweak, MsgColor.Red, MsgType.Hint);
            }
            if (n28 > 0)
            {
                if (n24 < 0)
                {
                    n24 = 0;
                }
                nDmg = M2Share.RandomNumber.Random(n24 * 10) + (n24 + 1) * 3;
                nDmg = GetHitStruckDamage(this, nDmg);
                StruckDamage(nDmg);
                SendRefMsg(Grobal2.RM_STRUCK, (short)nDmg, m_WAbil.HP, m_WAbil.MaxHP, 0, "");
            }
            return result;
        }

        private bool DoSpell(TUserMagic UserMagic, short nTargetX, short nTargetY, TBaseObject BaseObject)
        {
            var result = false;
            try
            {
                if (!M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx))
                {
                    var nSpellPoint = GetSpellPoint(UserMagic);
                    if (nSpellPoint > 0)
                    {
                        if (m_WAbil.MP < nSpellPoint)
                        {
                            return result;
                        }
                        DamageSpell(nSpellPoint);
                        HealthSpellChanged();
                    }
                    result = M2Share.MagicManager.DoSpell(this, UserMagic, nTargetX, nTargetY, BaseObject);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format("[Exception] TPlayObject.DoSpell MagID:{0} X:{1} Y:{2}", UserMagic.wMagIdx, nTargetX, nTargetY));
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 挖矿
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <returns></returns>
        private bool PileStones(int nX, int nY)
        {
            var result = false;
            var s1C = string.Empty;
            var mineEvent = (StoneMineEvent)m_PEnvir.GetEvent(nX, nY);
            if (mineEvent != null && mineEvent.EventType == Grobal2.ET_MINE)
            {
                if (mineEvent.MineCount > 0)
                {
                    mineEvent.MineCount -= 1;
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nMakeMineHitRate) == 0)
                    {
                        var pileEvent = (PileStones)m_PEnvir.GetEvent(m_nCurrX, m_nCurrY);
                        if (pileEvent == null)
                        {
                            pileEvent = new PileStones(m_PEnvir, m_nCurrX, m_nCurrY, Grobal2.ET_PILESTONES, 5 * 60 * 1000);
                            M2Share.EventManager.AddEvent(pileEvent);
                        }
                        else
                        {
                            if (pileEvent.EventType == Grobal2.ET_PILESTONES)
                            {
                                pileEvent.AddEventParam();
                            }
                        }
                        if (M2Share.RandomNumber.Random(M2Share.g_Config.nMakeMineRate) == 0)
                        {
                            if (m_PEnvir.Flag.boMINE)
                            {
                                MakeMine();
                            }
                            else if (m_PEnvir.Flag.boMINE2)
                            {
                                MakeMine2();
                            }
                        }
                        s1C = "1";
                        DoDamageWeapon(M2Share.RandomNumber.Random(15) + 5);
                        result = true;
                    }
                }
                else
                {
                    if ((HUtil32.GetTickCount() - mineEvent.AddStoneMineTick) > 10 * 60 * 1000)
                    {
                        mineEvent.AddStoneMine();
                    }
                }
            }
            SendRefMsg(Grobal2.RM_HEAVYHIT, Direction, m_nCurrX, m_nCurrY, 0, s1C);
            return result;
        }

        private void SendSaveItemList(int nBaseObject)
        {
            StdItem Item;
            TClientItem ClientItem = null;
            TUserItem UserItem;
            string sSendMsg = string.Empty;
            for (var i = 0; i < m_StorageItemList.Count; i++)
            {
                UserItem = m_StorageItemList[i];
                Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (Item != null)
                {
                    Item.GetStandardItem(ref ClientItem.Item);
                    Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                    ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientItem) + '/';
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SAVEITEMLIST, nBaseObject, 0, 0, (short)m_StorageItemList.Count);
            SendSocket(m_DefMsg, sSendMsg);
        }

        private void SendChangeGuildName()
        {
            if (m_MyGuild != null)
            {
                SendDefMessage(Grobal2.SM_CHANGEGUILDNAME, 0, 0, 0, 0, m_MyGuild.sGuildName + '/' + m_sGuildRankName);
            }
            else
            {
                SendDefMessage(Grobal2.SM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
            }
        }

        private void SendDelItemList(IList<TDeleteItem> ItemList)
        {
            var s10 = string.Empty;
            for (var i = 0; i < ItemList.Count; i++)
            {
                s10 = s10 + ItemList[i].sItemName + '/' + ItemList[i].MakeIndex + '/';
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELITEMS, 0, 0, 0, (short)ItemList.Count);
            SendSocket(m_DefMsg, EDcode.EncodeString(s10));
        }

        public void SendDelItems(TUserItem UserItem)
        {
            StdItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                TClientItem ClientItem = new TClientItem();
                StdItem.GetStandardItem(ref ClientItem.Item);
                StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                ClientItem.MakeIndex = UserItem.MakeIndex;
                ClientItem.Dura = UserItem.Dura;
                ClientItem.DuraMax = UserItem.DuraMax;
                if (StdItem.StdMode == 50)
                {
                    ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELITEM, ObjectId, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
            }
        }

        public void SendUpdateItem(TUserItem UserItem)
        {
            StdItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                TClientItem ClientItem = new TClientItem();
                StdItem.GetStandardItem(ref ClientItem.Item);
                StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                ClientItem.MakeIndex = UserItem.MakeIndex;
                ClientItem.Dura = UserItem.Dura;
                ClientItem.DuraMax = UserItem.DuraMax;
                if (StdItem.StdMode == 50)
                {
                    ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEITEM, ObjectId, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
            }
        }

        private bool CheckTakeOnItems(int nWhere, ref TClientStdItem StdItem)
        {
            var result = false;
            if (StdItem.StdMode == 10 && Gender != PlayGender.Man)
            {
                SysMsg(M2Share.sWearNotOfWoMan, MsgColor.Red, MsgType.Hint);
                return false;
            }
            if (StdItem.StdMode == 11 && Gender != PlayGender.WoMan)
            {
                SysMsg(M2Share.sWearNotOfMan, MsgColor.Red, MsgType.Hint);
                return false;
            }
            if (nWhere == 1 || nWhere == 2)
            {
                if (StdItem.Weight > m_WAbil.MaxHandWeight)
                {
                    SysMsg(M2Share.sHandWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            else
            {
                if (StdItem.Weight + GetUserItemWeitht(nWhere) > m_WAbil.MaxWearWeight)
                {
                    SysMsg(M2Share.sWearWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            switch (StdItem.Need)
            {
                case 0:
                    if (m_Abil.Level >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 1:
                    if (HUtil32.HiWord(m_WAbil.DC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 10:
                    if (m_btJob == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && m_Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 11:
                    if (m_btJob == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 12:
                    if (m_btJob == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 13:
                    if (m_btJob == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 2:
                    if (HUtil32.HiWord(m_WAbil.MC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 3:
                    if (HUtil32.HiWord(m_WAbil.SC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 4:
                    if (m_btReLevel >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 40:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (m_Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sLevelNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 41:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sDCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 42:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sMCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 43:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sSCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 44:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (m_btCreditPoint >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sCreditPointNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 5:
                    if (m_btCreditPoint >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sCreditPointNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 6:
                    if (m_MyGuild != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 60:
                    if (m_MyGuild != null && m_nGuildRankNo == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildMasterNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 7:
                    if (m_MyGuild != null && M2Share.CastleManager.IsCastleMember(this) != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSabukHumanNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 70:
                    if (m_MyGuild != null && M2Share.CastleManager.IsCastleMember(this) != null && m_nGuildRankNo == 1)
                    {
                        if (m_Abil.Level >= StdItem.NeedLevel)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sLevelNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSabukMasterManNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 8:
                    if (m_nMemberType != 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 81:
                    if (m_nMemberType == HUtil32.LoWord(StdItem.NeedLevel) && m_nMemberLevel >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberTypeNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 82:
                    if (m_nMemberType >= HUtil32.LoWord(StdItem.NeedLevel) && m_nMemberLevel >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberTypeNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
            }
            return result;
        }

        private int GetUserItemWeitht(int nWhere)
        {
            int result;
            var n14 = 0;
            StdItem StdItem;
            for (var i = 0; i < m_UseItems.Length; i++)
            {
                if (nWhere == -1 || !(i == nWhere) && !(i == 1) && !(i == 2))
                {
                    if (m_UseItems[i] == null)
                    {
                        continue;
                    }
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        n14 += StdItem.Weight;
                    }
                }
            }
            result = n14;
            return result;
        }

        private bool EatItems(StdItem StdItem, TUserItem Useritem)
        {
            var result = false;
            if (m_PEnvir.Flag.boNODRUG)
            {
                SysMsg(M2Share.sCanotUseDrugOnThisMap, MsgColor.Red, MsgType.Hint);
                return result;
            }
            switch (StdItem.StdMode)
            {
                case 0:
                    switch (StdItem.Shape)
                    {
                        case 1:
                            IncHealthSpell(StdItem.Ac, StdItem.Mac);
                            result = true;
                            break;
                        case 2:
                            m_boUserUnLockDurg = true;
                            result = true;
                            break;
                        case 3:
                            IncHealthSpell(HUtil32.Round(m_WAbil.MaxHP / 100 * StdItem.Ac), HUtil32.Round(m_WAbil.MaxMP / 100 * StdItem.Mac));
                            result = true;
                            break;
                        default:
                            if (StdItem.Ac > 0)
                            {
                                m_nIncHealth += StdItem.Ac;
                            }
                            if (StdItem.Mac > 0)
                            {
                                m_nIncSpell += StdItem.Mac;
                            }
                            result = true;
                            break;
                    }
                    break;
                case 1:
                    var nOldStatus = GetMyStatus();
                    m_nHungerStatus += StdItem.DuraMax / 10;
                    m_nHungerStatus = HUtil32._MIN(5000, m_nHungerStatus);
                    if (nOldStatus != GetMyStatus())
                    {
                        RefMyStatus();
                    }
                    result = true;
                    break;
                case 2:
                    result = true;
                    break;
                case 3:
                    switch (StdItem.Shape)
                    {
                        case 12:
                            var boNeedRecalc = false;
                            if (StdItem.Dc > 0)
                            {
                                m_wStatusArrValue[0] = StdItem.Dc;
                                m_dwStatusArrTimeOutTick[0] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("攻击力增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Mc > 0)
                            {
                                m_wStatusArrValue[1] = StdItem.Mc;
                                m_dwStatusArrTimeOutTick[1] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("魔法力增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Sc > 0)
                            {
                                m_wStatusArrValue[2] = StdItem.Sc;
                                m_dwStatusArrTimeOutTick[2] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("道术增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Ac2 > 0)
                            {
                                m_wStatusArrValue[3] = StdItem.Ac2;
                                m_dwStatusArrTimeOutTick[3] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("攻击速度增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Ac > 0)
                            {
                                m_wStatusArrValue[4] = StdItem.Ac;
                                m_dwStatusArrTimeOutTick[4] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("生命值增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Mac > 0)
                            {
                                m_wStatusArrValue[5] = StdItem.Mac;
                                m_dwStatusArrTimeOutTick[5] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("魔法值增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (boNeedRecalc)
                            {
                                RecalcAbilitys();
                                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                                result = true;
                            }
                            break;
                        case 13:
                            GetExp(StdItem.DuraMax);
                            result = true;
                            break;
                        default:
                            result = EatUseItems(StdItem.Shape);
                            break;
                    }
                    break;
            }
            return result;
        }

        private bool ReadBook(StdItem StdItem)
        {
            TUserMagic UserMagic;
            TPlayObject PlayObject;
            var result = false;
            var magic = M2Share.UserEngine.FindMagic(StdItem.Name);
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.wMagicID))
                {
                    if (magic.btJob == 99 || magic.btJob == (byte)m_btJob)
                    {
                        if (m_Abil.Level >= magic.TrainLevel[0])
                        {
                            UserMagic = new TUserMagic
                            {
                                MagicInfo = magic,
                                wMagIdx = magic.wMagicID,
                                btKey = 0,
                                btLevel = 0,
                                nTranPoint = 0
                            };
                            m_MagicList.Add(UserMagic);
                            RecalcAbilitys();
                            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                PlayObject = this;
                                PlayObject.SendAddMagic(UserMagic);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public void SendAddMagic(TUserMagic UserMagic)
        {
            var clientMagic = new TClientMagic
            {
                Key = (char)UserMagic.btKey,
                Level = UserMagic.btLevel,
                CurTrain = UserMagic.nTranPoint,
                Def = UserMagic.MagicInfo
            };
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDMAGIC, 0, 0, 0, 1);
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(clientMagic));
        }

        internal void SendDelMagic(TUserMagic UserMagic)
        {
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELMAGIC, UserMagic.wMagIdx, 0, 0, 1);
            SendSocket(m_DefMsg);
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="nShape"></param>
        /// <returns></returns>
        private bool EatUseItems(int nShape)
        {
            var result = false;
            switch (nShape)
            {
                case 1:
                    SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    BaseObjectMove(m_sHomeMap, 0, 0);
                    result = true;
                    break;
                case 2:
                    if (!m_PEnvir.Flag.boNORANDOMMOVE)
                    {
                        SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        BaseObjectMove(m_sMapName, 0, 0);
                        result = true;
                    }
                    break;
                case 3:
                    SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    if (PKLevel() < 2)
                    {
                        BaseObjectMove(m_sHomeMap, m_nHomeX, m_nHomeY);
                    }
                    else
                    {
                        BaseObjectMove(M2Share.g_Config.sRedHomeMap, M2Share.g_Config.nRedHomeX, M2Share.g_Config.nRedHomeY);
                    }
                    result = true;
                    break;
                case 4:
                    if (WeaptonMakeLuck())
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (m_MyGuild != null)
                    {
                        if (!m_boInFreePKArea)
                        {
                            TUserCastle Castle = M2Share.CastleManager.IsCastleMember(this);
                            if (Castle != null && Castle.IsMasterGuild(m_MyGuild))
                            {
                                BaseObjectMove(Castle.m_sHomeMap, Castle.GetHomeX(), Castle.GetHomeY());
                            }
                            else
                            {
                                SysMsg("无效", MsgColor.Red, MsgType.Hint);
                            }
                            result = true;
                        }
                        else
                        {
                            SysMsg("此处无法使用", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    break;
                case 9:
                    if (RepairWeapon())
                    {
                        result = true;
                    }
                    break;
                case 10:
                    if (SuperRepairWeapon())
                    {
                        result = true;
                    }
                    break;
                case 11:
                    WinLottery();
                    result = true;
                    break;
            }
            return result;
        }

        internal void MoveToHome()
        {
            SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            BaseObjectMove(m_sHomeMap, m_nHomeX, m_nHomeY);
        }

        private void BaseObjectMove(string sMap, short sX, short sY)
        {
            if (string.IsNullOrEmpty(sMap))
            {
                sMap = m_sMapName;
            }
            if (sX != 0 && sY != 0)
            {
                short nX = sX;
                short nY = sY;
                SpaceMove(sMap, nX, nY, 0);
            }
            else
            {
                MapRandomMove(sMap, 0);
            }
            var envir = m_PEnvir;
            if (envir != m_PEnvir && m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                m_boTimeRecall = false;
            }
        }

        private void ChangeServerMakeSlave(TSlaveInfo slaveInfo)
        {
            int nSlavecount = 0;
            if (m_btJob == PlayJob.Taoist)
            {
                nSlavecount = 1;
            }
            else
            {
                nSlavecount = 5;
            }
            var BaseObject = MakeSlave(slaveInfo.SlaveName, 3, slaveInfo.SlaveLevel, nSlavecount, slaveInfo.RoyaltySec);
            if (BaseObject != null)
            {
                BaseObject.m_nKillMonCount = slaveInfo.KillCount;
                BaseObject.m_btSlaveExpLevel = slaveInfo.SlaveExpLevel;
                BaseObject.m_WAbil.HP = slaveInfo.nHP;
                BaseObject.m_WAbil.MP = slaveInfo.nMP;
                if ((1500 - slaveInfo.SlaveLevel * 200) < BaseObject.m_nWalkSpeed)
                {
                    BaseObject.m_nWalkSpeed = (1500 - slaveInfo.SlaveLevel) * 200;
                }
                if ((2000 - slaveInfo.SlaveLevel * 200) < BaseObject.m_nNextHitTime)
                {
                    BaseObject.m_nWalkSpeed = (2000 - slaveInfo.SlaveLevel) * 200;
                }
                RecalcAbilitys();
            }
        }

        private void SendDelDealItem(TUserItem UserItem)
        {
            if (m_DealCreat != null)
            {
                StdItem pStdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (pStdItem != null)
                {
                    TClientItem ClientItem = new TClientItem();
                    pStdItem.GetStandardItem(ref ClientItem.Item);
                    ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEDELITEM, ObjectId, 0, 0, 1);
                    (m_DealCreat as TPlayObject)?.SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                    m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                    m_DealLastTick = HUtil32.GetTickCount();
                }
                SendDefMessage(Grobal2.SM_DEALDELITEM_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void SendAddDealItem(TUserItem UserItem)
        {
            SendDefMessage(Grobal2.SM_DEALADDITEM_OK, 0, 0, 0, 0, "");
            if (m_DealCreat != null)
            {
                StdItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    var ClientItem = new TClientItem();
                    StdItem.GetStandardItem(ref ClientItem.Item);
                    StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                    ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEADDITEM, ObjectId, 0, 0, 1);
                    (m_DealCreat as TPlayObject).SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                    m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                    m_DealLastTick = HUtil32.GetTickCount();
                }
            }
        }

        private void OpenDealDlg(TBaseObject BaseObject)
        {
            m_boDealing = true;
            m_DealCreat = BaseObject;
            GetBackDealItems();
            SendDefMessage(Grobal2.SM_DEALMENU, 0, 0, 0, 0, m_DealCreat.m_sCharName);
            m_DealLastTick = HUtil32.GetTickCount();
        }

        private void JoinGroup(TPlayObject PlayObject)
        {
            m_GroupOwner = PlayObject;
            SendGroupText(format(M2Share.g_sJoinGroup, m_sCharName));
        }

        /// <summary>
        /// 随机矿石持久度
        /// </summary>
        /// <returns></returns>
        private ushort MakeMineRandomDrua()
        {
            var result = M2Share.RandomNumber.Random(M2Share.g_Config.nStoneGeneralDuraRate) + M2Share.g_Config.nStoneMinDura;
            if (M2Share.RandomNumber.Random(M2Share.g_Config.nStoneAddDuraRate) == 0)
            {
                result += M2Share.RandomNumber.Random(M2Share.g_Config.nStoneAddDuraMax);
            }
            return (ushort)result;
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine()
        {
            TUserItem UserItem = null;
            if (m_ItemList.Count >= Grobal2.MAXBAGITEM)
            {
                return;
            }
            var nRandom = M2Share.RandomNumber.Random(M2Share.g_Config.nStoneTypeRate);
            if (nRandom >= M2Share.g_Config.nGoldStoneMin && nRandom <= M2Share.g_Config.nGoldStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGoldStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nSilverStoneMin && nRandom <= M2Share.g_Config.nSilverStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sSilverStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nSteelStoneMin && nRandom <= M2Share.g_Config.nSteelStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sSteelStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nBlackStoneMin && nRandom <= M2Share.g_Config.nBlackStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sBlackStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            UserItem = new TUserItem();
            if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sCopperStone, ref UserItem))
            {
                UserItem.Dura = MakeMineRandomDrua();
                m_ItemList.Add(UserItem);
                WeightChanged();
                SendAddItem(UserItem);
            }
            else
            {
                Dispose(UserItem);
            }
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine2()
        {
            if (m_ItemList.Count >= Grobal2.MAXBAGITEM)
            {
                return;
            }
            TUserItem mineItem = null;
            var mineRate = M2Share.RandomNumber.Random(120);
            if (HUtil32.RangeInDefined(mineRate, 1, 2))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone1, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else if (HUtil32.RangeInDefined(mineRate, 3, 20))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone2, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else if (HUtil32.RangeInDefined(mineRate, 21, 45))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone3, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone4, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
        }

        /// <summary>
        /// 检查任务物品
        /// </summary>
        /// <returns></returns>
        public TUserItem QuestCheckItem(string sItemName, ref int nCount, ref int nParam, ref int nDura)
        {
            string s1C;
            TUserItem result = null;
            nParam = 0;
            nDura = 0;
            nCount = 0;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                var UserItem = m_ItemList[i];
                s1C = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                if (string.Compare(s1C, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (UserItem.Dura > nDura)
                    {
                        nDura = UserItem.Dura;
                        result = UserItem;
                    }
                    nParam += UserItem.Dura;
                    if (result == null)
                    {
                        result = UserItem;
                    }
                    nCount++;
                }
            }
            return result;
        }

        public bool QuestTakeCheckItem(TUserItem CheckItem)
        {
            TUserItem UserItem;
            var result = false;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem == CheckItem)
                {
                    SendDelItems(UserItem);
                    Dispose(UserItem);
                    m_ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < m_UseItems.Length; i++)
            {
                if (m_UseItems[i] == CheckItem)
                {
                    SendDelItems(m_UseItems[i]);
                    m_UseItems[i].wIndex = 0;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void MakeSaveRcd(ref THumDataInfo HumanRcd)
        {
            var HumData = HumanRcd.Data;
            HumData.sCharName = m_sCharName;
            HumData.sCurMap = m_sMapName;
            HumData.wCurX = m_nCurrX;
            HumData.wCurY = m_nCurrY;
            HumData.btDir = Direction;
            HumData.btHair = m_btHair;
            HumData.btSex = (byte)Gender;
            HumData.btJob = (byte)m_btJob;
            HumData.nGold = m_nGold;
            HumData.Abil.Level = m_Abil.Level;
            HumData.Abil.HP = m_Abil.HP;
            HumData.Abil.MP = m_Abil.MP;
            HumData.Abil.MaxHP = m_Abil.MaxHP;
            HumData.Abil.MaxMP = m_Abil.MaxMP;
            HumData.Abil.Exp = m_Abil.Exp;
            HumData.Abil.MaxExp = m_Abil.MaxExp;
            HumData.Abil.Weight = m_Abil.Weight;
            HumData.Abil.MaxWeight = m_Abil.MaxWeight;
            HumData.Abil.WearWeight = m_Abil.WearWeight;
            HumData.Abil.MaxWearWeight = m_Abil.MaxWearWeight;
            HumData.Abil.HandWeight = m_Abil.HandWeight;
            HumData.Abil.MaxHandWeight = m_Abil.MaxHandWeight;
            HumData.Abil.HP = m_WAbil.HP;
            HumData.Abil.MP = m_WAbil.MP;
            HumData.wStatusTimeArr = m_wStatusTimeArr;
            HumData.sHomeMap = m_sHomeMap;
            HumData.wHomeX = m_nHomeX;
            HumData.wHomeY = m_nHomeY;
            HumData.nPKPoint = m_nPkPoint;
            HumData.BonusAbil = m_BonusAbil;
            HumData.nBonusPoint = m_nBonusPoint;
            HumData.sStoragePwd = m_sStoragePwd;
            HumData.btCreditPoint = m_btCreditPoint;
            HumData.btReLevel = m_btReLevel;
            HumData.sMasterName = m_sMasterName;
            HumData.boMaster = m_boMaster;
            HumData.sDearName = m_sDearName;
            HumData.nGameGold = m_nGameGold;
            HumData.nGamePoint = m_nGamePoint;
            if (m_boAllowGroup)
            {
                HumData.btAllowGroup = 1;
            }
            else
            {
                HumData.btAllowGroup = 0;
            }
            HumData.btF9 = btB2;
            HumData.btAttatckMode = (byte)m_btAttatckMode;
            HumData.btIncHealth = (byte)m_nIncHealth;
            HumData.btIncSpell = (byte)m_nIncSpell;
            HumData.btIncHealing = (byte)m_nIncHealing;
            HumData.btFightZoneDieCount = (byte)m_nFightZoneDieCount;
            HumData.sAccount = m_sUserID;
            HumData.btEE = (byte)nC4;
            HumData.boLockLogon = m_boLockLogon;
            HumData.wContribution = m_wContribution;
            HumData.btEF = btC8;
            HumData.nHungerStatus = m_nHungerStatus;
            HumData.boAllowGuildReCall = m_boAllowGuildReCall;
            HumData.wGroupRcallTime = m_wGroupRcallTime;
            HumData.dBodyLuck = m_dBodyLuck;
            HumData.boAllowGroupReCall = m_boAllowGroupReCall;
            HumData.QuestUnitOpen = m_QuestUnitOpen;
            HumData.QuestUnit = m_QuestUnit;
            HumData.QuestFlag = m_QuestFlag;
            var HumItems = HumanRcd.Data.HumItems;
            if (HumItems == null)
            {
                HumItems = new TUserItem[13];
            }
            HumItems[Grobal2.U_DRESS] = m_UseItems[Grobal2.U_DRESS] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_DRESS];
            HumItems[Grobal2.U_WEAPON] = m_UseItems[Grobal2.U_WEAPON] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_WEAPON];
            HumItems[Grobal2.U_RIGHTHAND] = m_UseItems[Grobal2.U_RIGHTHAND] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_RIGHTHAND];
            HumItems[Grobal2.U_HELMET] = m_UseItems[Grobal2.U_NECKLACE] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_NECKLACE];
            HumItems[Grobal2.U_NECKLACE] = m_UseItems[Grobal2.U_HELMET] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_HELMET];
            HumItems[Grobal2.U_ARMRINGL] = m_UseItems[Grobal2.U_ARMRINGL] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_ARMRINGL];
            HumItems[Grobal2.U_ARMRINGR] = m_UseItems[Grobal2.U_ARMRINGR] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_ARMRINGR];
            HumItems[Grobal2.U_RINGL] = m_UseItems[Grobal2.U_RINGL] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_RINGL];
            HumItems[Grobal2.U_RINGR] = m_UseItems[Grobal2.U_RINGR] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_RINGR];
            HumItems[Grobal2.U_BUJUK] = m_UseItems[Grobal2.U_BUJUK] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_BUJUK];
            HumItems[Grobal2.U_BELT] = m_UseItems[Grobal2.U_BELT] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_BELT];
            HumItems[Grobal2.U_BOOTS] = m_UseItems[Grobal2.U_BOOTS] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_BOOTS];
            HumItems[Grobal2.U_CHARM] = m_UseItems[Grobal2.U_CHARM] == null ? HUtil32.DelfautItem : m_UseItems[Grobal2.U_CHARM];
            var BagItems = HumanRcd.Data.BagItems;
            if (BagItems == null)
            {
                BagItems = new TUserItem[46];
            }
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                if (i <= 46)
                {
                    BagItems[i] = m_ItemList[i];
                }
            }
            for (int i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    BagItems[i] = HUtil32.DelfautItem;
                }
            }
            var HumMagic = HumanRcd.Data.Magic;
            if (HumMagic == null)
            {
                HumMagic = new TMagicRcd[Grobal2.MaxMagicCount];
            }
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                if (i >= Grobal2.MaxMagicCount)
                {
                    break;
                }
                var UserMagic = m_MagicList[i];
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = new TMagicRcd();
                }
                HumMagic[i].wMagIdx = UserMagic.wMagIdx;
                HumMagic[i].btLevel = UserMagic.btLevel;
                HumMagic[i].btKey = UserMagic.btKey;
                HumMagic[i].nTranPoint = UserMagic.nTranPoint;
            }
            for (int i = 0; i < HumMagic.Length; i++)
            {
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = HUtil32.DetailtMagicRcd;
                }
            }
            var StorageItems = HumanRcd.Data.StorageItems;
            if (StorageItems == null)
            {
                StorageItems = new TUserItem[50];
            }
            for (var i = 0; i < this.m_StorageItemList.Count; i++)
            {
                if (i >= StorageItems.Length)
                {
                    break;
                }
                StorageItems[i] = this.m_StorageItemList[i];
            }
            for (int i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = HUtil32.DelfautItem;
                }
            }
        }

        public void RefRankInfo(int nRankNo, string sRankName)
        {
            m_nGuildRankNo = nRankNo;
            m_sGuildRankName = sRankName;
            SendMsg(this, Grobal2.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
        }

        /// <summary>
        /// 攻击消息数量
        /// </summary>
        /// <returns></returns>
        private int GetHitMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent >= Grobal2.CM_HIT || m_MsgList[i].wIdent <= Grobal2.CM_FIREHIT)
                    {
                        result++;
                    }
                    //if (SendMessage.wIdent == Grobal2.CM_HIT || SendMessage.wIdent == Grobal2.CM_HEAVYHIT || SendMessage.wIdent == Grobal2.CM_BIGHIT || SendMessage.wIdent == Grobal2.CM_POWERHIT
                    //    || SendMessage.wIdent == Grobal2.CM_LONGHIT || SendMessage.wIdent == Grobal2.CM_WIDEHIT || SendMessage.wIdent == Grobal2.CM_FIREHIT)
                    //{
                    //     result++;
                    //}
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 魔法消息数量
        /// </summary>
        /// <returns></returns>
        private int GetSpellMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent == Grobal2.CM_SPELL)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 跑步消息数量
        /// </summary>
        /// <returns></returns>
        private int GetRunMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent == Grobal2.CM_RUN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 走路消息数量
        /// </summary>
        /// <returns></returns>
        private int GetWalkMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent == Grobal2.CM_WALK)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private int GetTurnMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent == Grobal2.CM_TURN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private int GetSiteDownMsgCount()
        {
            var result = 0;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    if (m_MsgList[i].wIdent == Grobal2.CM_SITDOWN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private bool CheckActionStatus(int wIdent, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            if (M2Share.g_Config.boSpeedHackCheck)
            {
                return true;
            }
            int dwCheckTime;
            if (!M2Share.g_Config.boDisableStruck) // 检查人物弯腰停留时间
            {
                dwCheckTime = HUtil32.GetTickCount() - m_dwStruckTick;
                if (M2Share.g_Config.dwStruckTime > dwCheckTime)
                {
                    dwDelayTime = M2Share.g_Config.dwStruckTime - dwCheckTime;
                    m_btOldDir = Direction;
                    return false;
                }
            }
            // 检查二个不同操作之间所需间隔时间
            dwCheckTime = HUtil32.GetTickCount() - m_dwActionTick;
            if (m_boTestSpeedMode)
            {
                SysMsg("间隔: " + dwCheckTime, MsgColor.Blue, MsgType.Notice);
            }
            if (m_wOldIdent == wIdent)
            {
                // 当二次操作一样时，则将 boFirst 设置为 真 ，退出由调用函数本身检查二个相同操作之间的间隔时间
                return true;
            }
            if (!M2Share.g_Config.boControlActionInterval)
            {
                return true;
            }
            int dwActionIntervalTime = m_dwActionIntervalTime;
            switch (wIdent)
            {
                case Grobal2.CM_LONGHIT:
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_HIT:
                    if (M2Share.g_Config.boControlWalkHit && m_wOldIdent == Grobal2.CM_WALK && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime; // 走位攻击
                    }
                    if (M2Share.g_Config.boControlRunHit && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    break;
                case Grobal2.CM_RUN:
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_LONGHIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    if (M2Share.g_Config.boControlRunHit && m_wOldIdent == Grobal2.CM_HIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    if (M2Share.g_Config.boControlRunMagic && m_wOldIdent == Grobal2.CM_SPELL && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
                case Grobal2.CM_WALK:
                    if (M2Share.g_Config.boControlWalkHit && m_wOldIdent == Grobal2.CM_HIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime;// 走位攻击
                    }
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_LONGHIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_SPELL:
                    if (M2Share.g_Config.boControlRunMagic && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
            }
            // 将几个攻击操作合并成一个攻击操作代码
            if (wIdent == Grobal2.CM_HIT || wIdent == Grobal2.CM_HEAVYHIT || wIdent == Grobal2.CM_BIGHIT || wIdent == Grobal2.CM_POWERHIT || wIdent == Grobal2.CM_WIDEHIT || wIdent == Grobal2.CM_FIREHIT)
            {
                wIdent = Grobal2.CM_HIT;
            }
            if (dwCheckTime >= dwActionIntervalTime)
            {
                m_dwActionTick = HUtil32.GetTickCount();
                result = true;
            }
            else
            {
                dwDelayTime = dwActionIntervalTime - dwCheckTime;
            }
            m_wOldIdent = wIdent;
            m_btOldDir = Direction;
            return result;
        }

        public void SetScriptLabel(string sLabel)
        {
            m_CanJmpScriptLableList.Clear();
            m_CanJmpScriptLableList.Add(sLabel, sLabel);
        }

        /// <summary>
        /// 取得当前脚本可以跳转的标签
        /// </summary>
        /// <param name="sMsg"></param>
        public void GetScriptLabel(string sMsg)
        {
            var sText = string.Empty;
            m_CanJmpScriptLableList.Clear();
            const string start = "<";
            const string end = ">";
            var rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            while (true)
            {
                if (string.IsNullOrEmpty(sMsg))
                {
                    break;
                }
                sMsg = HUtil32.GetValidStr3(sMsg, ref sText, "\\");
                if (!string.IsNullOrEmpty(sText))
                {
                    var match = rg.Matches(sText);
                    if (match.Count > 0)
                    {
                        foreach (Match item in match)
                        {
                            var sCmdStr = item.Value;
                            var sLabel = HUtil32.GetValidStr3(sCmdStr, ref sCmdStr, HUtil32.Backslash);
                            if (!string.IsNullOrEmpty(sLabel) && !m_CanJmpScriptLableList.ContainsKey(sLabel))
                            {
                                m_CanJmpScriptLableList.Add(sLabel, sLabel);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 脚本标签是否可以跳转
        /// </summary>
        /// <param name="sLabel"></param>
        /// <returns></returns>
        public bool LableIsCanJmp(string sLabel)
        {
            if (string.Compare(sLabel, "@main", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            if (m_CanJmpScriptLableList.ContainsKey(sLabel))
            {
                return true;
            }
            if (string.Compare(sLabel, m_sPlayDiceLabel, StringComparison.OrdinalIgnoreCase) == 0)
            {
                m_sPlayDiceLabel = string.Empty;
                return true;
            }
            return false;
        }

        private bool CheckItemsNeed(StdItem StdItem)
        {
            var result = true;
            var castle = M2Share.CastleManager.IsCastleMember(this);
            switch (StdItem.Need)
            {
                case 6:
                    if (m_MyGuild == null)
                    {
                        result = false;
                    }
                    break;
                case 60:
                    if (m_MyGuild == null || m_nGuildRankNo != 1)
                    {
                        result = false;
                    }
                    break;
                case 7:
                    if (castle == null)
                    {
                        result = false;
                    }
                    break;
                case 70:
                    if (castle == null || m_nGuildRankNo != 1)
                    {
                        result = false;
                    }
                    break;
                case 8:
                    if (m_nMemberType == 0)
                    {
                        result = false;
                    }
                    break;
                case 81:
                    if (m_nMemberType != HUtil32.LoWord(StdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
                case 82:
                    if (m_nMemberType < HUtil32.LoWord(StdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
            }
            return result;
        }

        private void CheckMarry()
        {
            StringList LoadList;
            string sSayMsg;
            var boIsfound = false;
            var sUnMarryFileName = M2Share.sConfigPath + M2Share.g_Config.sEnvirDir + "UnMarry.txt";
            if (File.Exists(sUnMarryFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sUnMarryFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    if (string.Compare(LoadList[i], this.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        LoadList.RemoveAt(i);
                        boIsfound = true;
                        break;
                    }
                }
                LoadList.SaveToFile(sUnMarryFileName);
                LoadList.Dispose();
                LoadList = null;
            }
            if (boIsfound)
            {
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMarryManLoginMsg, m_sDearName, m_sDearName);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMarryWoManLoginMsg, m_sCharName, m_sCharName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                m_sDearName = "";
                RefShowName();
            }
            m_DearHuman = M2Share.UserEngine.GetPlayObject(m_sDearName);
            if (m_DearHuman != null)
            {
                m_DearHuman.m_DearHuman = this;
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineSelfMsg, m_sDearName, m_sCharName, m_DearHuman.m_PEnvir.MapDesc, m_DearHuman.m_nCurrX, m_DearHuman.m_nCurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineDearMsg, m_sDearName, m_sCharName, m_PEnvir.MapDesc, m_nCurrX, m_nCurrY);
                    m_DearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineSelfMsg, m_sDearName, m_sCharName, m_DearHuman.m_PEnvir.MapDesc, m_DearHuman.m_nCurrX, m_DearHuman.m_nCurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineDearMsg, m_sDearName, m_sCharName, m_PEnvir.MapDesc, m_nCurrX, m_nCurrY);
                    m_DearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
            }
            else
            {
                if (Gender == PlayGender.Man)
                {
                    SysMsg(M2Share.g_sManLoginDearNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    SysMsg(M2Share.g_sWoManLoginDearNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private void CheckMaster()
        {
            bool boIsfound = false;
            string sSayMsg;
            TPlayObject Human;
            for (var i = 0; i < M2Share.g_UnForceMasterList.Count; i++) // 处理强行脱离师徒关系
            {
                if (string.Compare(M2Share.g_UnForceMasterList[i], this.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.g_UnForceMasterList.RemoveAt(i);
                    M2Share.SaveUnForceMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound)
            {
                if (m_boMaster)
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMasterLoginMsg, m_sMasterName);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMasterListLoginMsg, m_sMasterName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                m_sMasterName = "";
                RefShowName();
            }
            if (!string.IsNullOrEmpty(m_sMasterName) && !m_boMaster)
            {
                if (m_Abil.Level >= M2Share.g_Config.nMasterOKLevel)
                {
                    Human = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                    if (Human != null && !Human.m_boDeath && !Human.m_boGhost)
                    {
                        sSayMsg = string.Format(M2Share.g_sYourMasterListUnMasterOKMsg, m_sCharName);
                        Human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        if (m_sCharName == Human.m_sMasterName)// 如果大徒弟则将师父上的名字去掉
                        {
                            Human.m_sMasterName = "";
                            Human.RefShowName();
                        }
                        for (var i = 0; i < Human.m_MasterList.Count; i++)
                        {
                            if (Human.m_MasterList[i] == this)
                            {
                                Human.m_MasterList.RemoveAt(i);
                                break;
                            }
                        }
                        m_sMasterName = "";
                        RefShowName();
                        if (Human.m_btCreditPoint + M2Share.g_Config.nMasterOKCreditPoint <= byte.MaxValue)
                        {
                            Human.m_btCreditPoint += (byte)M2Share.g_Config.nMasterOKCreditPoint;
                        }
                        Human.m_nBonusPoint += M2Share.g_Config.nMasterOKBonusPoint;
                        Human.SendMsg(Human, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        // 如果师父不在线则保存到记录表中
                        boIsfound = false;
                        for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
                        {
                            if (string.Compare(M2Share.g_UnMasterList[i], this.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                boIsfound = true;
                                break;
                            }
                        }
                        if (!boIsfound)
                        {
                            M2Share.g_UnMasterList.Add(m_sMasterName);
                        }
                        if (!boIsfound)
                        {
                            M2Share.SaveUnMasterList();
                        }
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        m_sMasterName = "";
                        RefShowName();
                    }
                }
            }
            // 处理出师记录
            boIsfound = false;
            for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
            {
                if (string.Compare(M2Share.g_UnMasterList[i], this.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.g_UnMasterList.RemoveAt(i);
                    M2Share.SaveUnMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound && m_boMaster)
            {
                SysMsg(M2Share.g_sUnMasterLoginMsg, MsgColor.Red, MsgType.Hint);
                m_sMasterName = "";
                RefShowName();
                if (m_btCreditPoint + M2Share.g_Config.nMasterOKCreditPoint <= byte.MaxValue)
                {
                    m_btCreditPoint += (byte)M2Share.g_Config.nMasterOKCreditPoint;
                }
                m_nBonusPoint += M2Share.g_Config.nMasterOKBonusPoint;
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            }
            if (string.IsNullOrEmpty(m_sMasterName))
            {
                return;
            }
            if (m_boMaster) // 师父上线通知
            {
                m_MasterHuman = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                if (m_MasterHuman != null)
                {
                    m_MasterHuman.m_MasterHuman = this;
                    m_MasterList.Add(m_MasterHuman);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineSelfMsg, m_sMasterName, m_sCharName, m_MasterHuman.m_PEnvir.MapDesc, m_MasterHuman.m_nCurrX, m_MasterHuman.m_nCurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineMasterListMsg, m_sMasterName, m_sCharName, m_PEnvir.MapDesc, m_nCurrX, m_nCurrY);
                    m_MasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    SysMsg(M2Share.g_sMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                // 徒弟上线通知
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    m_MasterHuman = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                    if (m_MasterHuman != null)
                    {
                        if (m_MasterHuman.m_sMasterName == m_sCharName)
                        {
                            m_MasterHuman.m_MasterHuman = this;
                        }
                        m_MasterHuman.m_MasterList.Add(this);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineSelfMsg, m_sMasterName, m_sCharName, m_MasterHuman.m_PEnvir.MapDesc, m_MasterHuman.m_nCurrX, m_MasterHuman.m_nCurrY);
                        SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineMasterMsg, m_sMasterName, m_sCharName, m_PEnvir.MapDesc, m_nCurrX, m_nCurrY);
                        m_MasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    }
                }
            }
        }

        public string GetMyInfo()
        {
            var sMyInfo = M2Share.g_sMyInfo;
            sMyInfo = sMyInfo.Replace("%name", m_sCharName);
            sMyInfo = sMyInfo.Replace("%map", m_PEnvir.MapDesc);
            sMyInfo = sMyInfo.Replace("%x", m_nCurrX.ToString());
            sMyInfo = sMyInfo.Replace("%y", m_nCurrY.ToString());
            sMyInfo = sMyInfo.Replace("%level", m_Abil.Level.ToString());
            sMyInfo = sMyInfo.Replace("%gold", m_nGold.ToString());
            sMyInfo = sMyInfo.Replace("%pk", m_nPkPoint.ToString());
            sMyInfo = sMyInfo.Replace("%minhp", m_WAbil.HP.ToString());
            sMyInfo = sMyInfo.Replace("%maxhp", m_WAbil.MaxHP.ToString());
            sMyInfo = sMyInfo.Replace("%minmp", m_WAbil.MP.ToString());
            sMyInfo = sMyInfo.Replace("%maxmp", m_WAbil.MaxMP.ToString());
            sMyInfo = sMyInfo.Replace("%mindc", HUtil32.LoWord(m_WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%maxdc", HUtil32.HiWord(m_WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%minmc", HUtil32.LoWord(m_WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%maxmc", HUtil32.HiWord(m_WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%minsc", HUtil32.LoWord(m_WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%maxsc", HUtil32.HiWord(m_WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%logontime", m_dLogonTime.ToString());
            sMyInfo = sMyInfo.Replace("%logonint", ((HUtil32.GetTickCount() - m_dwLogonTick) / 60000).ToString());
            return sMyInfo;
        }

        private bool CheckItemBindUse(TUserItem UserItem)
        {
            TItemBind ItemBind;
            bool result = true;
            for (var i = 0; i < M2Share.g_ItemBindAccount.Count; i++)
            {
                ItemBind = M2Share.g_ItemBindAccount[i];
                if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                {
                    result = false;
                    if (string.Compare(ItemBind.sBindName, m_sUserID, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sItemIsNotThisAccount, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            for (var i = 0; i < M2Share.g_ItemBindIPaddr.Count; i++)
            {
                ItemBind = M2Share.g_ItemBindIPaddr[i];
                if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                {
                    result = false;
                    if (string.Compare(ItemBind.sBindName, m_sIPaddr, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sItemIsNotThisIPaddr, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            for (var i = 0; i < M2Share.g_ItemBindCharName.Count; i++)
            {
                ItemBind = M2Share.g_ItemBindCharName[i];
                if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                {
                    result = false;
                    if (string.Compare(ItemBind.sBindName, m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sItemIsNotThisCharName, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            return result;
        }

        private void ProcessClientPassword(TProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wParam == 0)
            {
                ProcessUserLineMsg("@" + M2Share.g_GameCommand.UNLOCK.sCmd);
                return;
            }
            string sData = ProcessMsg.sMsg;
            int nLen = sData.Length;
            if (m_boSetStoragePwd)
            {
                m_boSetStoragePwd = false;
                if (nLen > 3 && nLen < 8)
                {
                    m_sTempPwd = sData;
                    m_boReConfigPwd = true;
                    SysMsg(M2Share.g_sReSetPasswordMsg, MsgColor.Green, MsgType.Hint);// '请重复输入一次仓库密码：'
                    SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                }
                else
                {
                    SysMsg(M2Share.g_sPasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                }
                return;
            }
            if (m_boReConfigPwd)
            {
                m_boReConfigPwd = false;
                if (string.Compare(m_sTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_sStoragePwd = sData;
                    m_boPasswordLocked = true;
                    m_sTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);// '密码设置成功!!，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。'
                }
                else
                {
                    m_sTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (m_boUnLockPwd || m_boUnLockStoragePwd)
            {
                if (string.Compare(m_sStoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_boPasswordLocked = false;
                    if (m_boUnLockPwd)
                    {
                        if (M2Share.g_Config.boLockDealAction)
                        {
                            m_boCanDeal = true;
                        }
                        if (M2Share.g_Config.boLockDropAction)
                        {
                            m_boCanDrop = true;
                        }
                        if (M2Share.g_Config.boLockWalkAction)
                        {
                            m_boCanWalk = true;
                        }
                        if (M2Share.g_Config.boLockRunAction)
                        {
                            m_boCanRun = true;
                        }
                        if (M2Share.g_Config.boLockHitAction)
                        {
                            m_boCanHit = true;
                        }
                        if (M2Share.g_Config.boLockSpellAction)
                        {
                            m_boCanSpell = true;
                        }
                        if (M2Share.g_Config.boLockSendMsgAction)
                        {
                            m_boCanSendMsg = true;
                        }
                        if (M2Share.g_Config.boLockUserItemAction)
                        {
                            m_boCanUseItem = true;
                        }
                        if (M2Share.g_Config.boLockInObModeAction)
                        {
                            m_boObMode = false;
                            m_boAdminMode = false;
                        }
                        m_boLockLogoned = true;
                        SysMsg(M2Share.g_sPasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    if (m_boUnLockStoragePwd)
                    {
                        if (M2Share.g_Config.boLockGetBackItemAction)
                        {
                            m_boCanGetBackItem = true;
                        }
                        SysMsg(M2Share.g_sStorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                }
                else
                {
                    m_btPwdFailCount++;
                    SysMsg(M2Share.g_sUnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                    }
                }
                m_boUnLockPwd = false;
                m_boUnLockStoragePwd = false;
                return;
            }
            if (m_boCheckOldPwd)
            {
                m_boCheckOldPwd = false;
                if (m_sStoragePwd == sData)
                {
                    SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                    m_boSetStoragePwd = true;
                }
                else
                {
                    m_btPwdFailCount++;
                    SysMsg(M2Share.g_sOldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                        m_boPasswordLocked = true;
                    }
                }
            }
        }

        public void RecallHuman(string sHumName)
        {
            short nX = 0;
            short nY = 0;
            short n18 = 0;
            short n1C = 0;
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (GetFrontPosition(ref nX, ref nY))
                {
                    if (GetRecallXY(nX, nY, 3, ref n18, ref n1C))
                    {
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(m_sMapName, n18, n1C, 0);
                    }
                }
                else
                {
                    SysMsg("召唤失败!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                SysMsg(format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }

        public void ReQuestGuildWar(string sGuildName)
        {
            if (!IsGuildMaster())
            {
                SysMsg("只有行会掌门人才能申请!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                SysMsg("这个命令不能在本服务器上使用!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            GuildInfo Guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (Guild == null)
            {
                SysMsg("行会不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boReQuestOK = false;
            TWarGuild WarGuild = m_MyGuild.AddWarGuild(Guild);
            if (WarGuild != null)
            {
                if (Guild.AddWarGuild(m_MyGuild) == null)
                {
                    WarGuild.dwWarTick = 0;
                }
                else
                {
                    boReQuestOK = true;
                }
            }
            if (boReQuestOK)
            {
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, Guild.sGuildName);
            }
        }

        private bool CheckDenyLogon()
        {
            var result = false;
            if (M2Share.GetDenyIPAddrList(m_sIPaddr))
            {
                SysMsg(M2Share.g_sYourIPaddrDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyAccountList(m_sUserID))
            {
                SysMsg(M2Share.g_sYourAccountDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyChrNameList(m_sCharName))
            {
                SysMsg(M2Share.g_sYourCharNameDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            if (result)
            {
                m_boEmergencyClose = true;
            }
            return result;
        }

        /// <summary>
        /// 转移到指定服务器
        /// </summary>
        /// <param name="sIPaddr"></param>
        /// <param name="nPort"></param>
        public void ChangeSnapsServer(string sIPaddr, int nPort)
        {
            this.SendMsg(this, Grobal2.RM_RECONNECTION, 0, 0, 0, 0, sIPaddr + '/' + nPort);
        }
    }
}