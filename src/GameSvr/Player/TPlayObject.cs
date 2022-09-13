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
using SystemModule.Packet.ServerPackets;
using StdItem = GameSvr.Items.StdItem;

namespace GameSvr.Player
{
    public partial class PlayObject : AnimalObject
    {
        private bool ClientPickUpItem_IsSelf(int actorId)
        {
            bool result;
            if (actorId == 0 || this.ActorId == actorId)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ClientPickUpItem_IsOfGroup(int actorId)
        {
            if (GroupOwner == null)
            {
                return false;
            }
            for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
            {
                if (GroupOwner.GroupMembers[i].ActorId == actorId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ClientPickUpItem()
        {
            var result = false;
            if (Dealing)
            {
                return false;
            }
            var mapItem = Envir.GetItem(CurrX, CurrY);
            if (mapItem == null)
            {
                return false;
            }
            if (HUtil32.GetTickCount() - mapItem.CanPickUpTick > M2Share.Config.FloorItemCanPickUpTime)// 2 * 60 * 1000
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
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.ItemObject, mapItem) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(Grobal2.RM_ITEMHIDE, 0, mapItem.ActorId, CurrX, CurrY, "");
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('4' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + Grobal2.sSTRING_GOLDNAME
                                                   + "\t" + mapItem.Count + "\t" + '1' + "\t" + '0');
                        }
                        GoldChanged();
                        Dispose(mapItem);
                    }
                    else
                    {
                        Envir.AddToMap(CurrX, CurrY, CellType.ItemObject, mapItem);
                    }
                }
                return result;
            }
            if (IsEnoughBag())
            {
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.ItemObject, mapItem) == 1)
                {
                    var UserItem = mapItem.UserItem;
                    var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(UserItem.wIndex)))
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, mapItem.ActorId, CurrX, CurrY, "");
                        AddItemToBag(UserItem);
                        if (!M2Share.IsCheapStuff(StdItem.StdMode))
                        {
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('4' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name
                                                       + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        Dispose(mapItem);
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            this.SendAddItem(UserItem);
                        }
                        result = true;
                    }
                    else
                    {
                        Dispose(UserItem);
                        Envir.AddToMap(CurrX, CurrY, CellType.ItemObject, mapItem);
                    }
                }
            }
            return result;
        }

        private void WinExp(int dwExp)
        {
            if (Abil.Level > M2Share.Config.LimitExpLevel)
            {
                dwExp = M2Share.Config.LimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = M2Share.Config.KillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = m_nKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(m_nKillMonExpRate / 100 * dwExp);// 人物指定的杀怪经验倍数
                if (Envir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(Envir.Flag.nEXPRATE / 100 * dwExp);// 地图上指定杀怪经验倍数
                }
                if (BoExpItem) // 物品经验倍数
                {
                    dwExp = HUtil32.Round(ExpItem * dwExp);
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, Grobal2.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (Abil.Exp >= Abil.MaxExp)
            {
                Abil.Exp -= Abil.MaxExp;
                if (Abil.Level < M2Share.MAXUPLEVEL)
                {
                    Abil.Level++;
                }
                HasLevelUp(Abil.Level - 1);
                AddBodyLuck(100);
                M2Share.AddGameDataLog("12" + "\t" + MapName + "\t" + Abil.Level + "\t" + Abil.Exp + "\t" + CharName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public bool IncGold(int tGold)
        {
            var result = false;
            if (Gold + tGold <= M2Share.Config.HumanMaxGold)
            {
                Gold += tGold;
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
            return ItemList.Count < Grobal2.MAXBAGITEM;
        }

        /// <summary>
        /// 是否超过包裹最大负重
        /// </summary>
        /// <param name="nWeight"></param>
        /// <returns></returns>
        public bool IsAddWeightAvailable(int nWeight)
        {
            return Abil.Weight + nWeight <= Abil.MaxWeight;
        }

        public void SendAddItem(UserItem UserItem)
        {
            var Item = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
            if (Item == null)
            {
                return;
            }
            var ClientItem = new ClientItem();
            Item.GetStandardItem(ref ClientItem.Item);
            Item.GetItemAddValue(UserItem, ref ClientItem.Item);
            ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
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
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDITEM, ActorId, 0, 0, 1);
            SendSocket(m_DefMsg, EDCode.EncodeBuffer(ClientItem));
        }

        internal bool IsBlockWhisper(string sName)
        {
            var result = false;
            for (var i = 0; i < this.LockWhisperList.Count; i++)
            {
                if (string.Compare(sName, this.LockWhisperList[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        internal void SendSocket(string sMsg)
        {
            if (OffLineFlag)
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
                MsgHdr.PackLength = -bMsg.Length;
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
                M2Share.GateMgr.AddGateBuffer(m_nGateIdx, data);
            }
        }

        private void SendSocket(ClientPacket DefMsg)
        {
            SendSocket(DefMsg, "");
        }

        internal virtual void SendSocket(ClientPacket defMsg, string sMsg)
        {
            if (OffLineFlag && defMsg.Ident != Grobal2.SM_OUTOFCONNECTION)
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
                messageHead.PackLength = -bMsg.Length;
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
            M2Share.GateMgr.AddGateBuffer(m_nGateIdx, data);
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            m_DefMsg = Grobal2.MakeDefaultMsg(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(m_DefMsg, EDCode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(m_DefMsg);
            }
        }

        private byte DayBright()
        {
            byte result;
            if (Envir.Flag.boDarkness)
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
            if (Envir.Flag.boDayLight)
            {
                result = 0;
            }
            return result;
        }

        private void RefUserState()
        {
            var n8 = 0;
            if (Envir.Flag.boFightZone)
            {
                n8 = n8 | 1;
            }
            if (Envir.Flag.boSAFE)
            {
                n8 = n8 | 2;
            }
            if (InFreePkArea)
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
            if (!M2Share.Config.SpiritMutiny || !MBopirit)
            {
                return;
            }
            MBopirit = false;
            for (var i = 0; i < UseItems.Length; i++)
            {
                var UseItem = UseItems[i];
                if (UseItem == null)
                {
                    continue;
                }
                if (UseItem.wIndex <= 0)
                {
                    continue;
                }
                var StdItem = M2Share.WorldEngine.GetStdItem(UseItem.wIndex);
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
            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + M2Share.Config.SpiritMutinyTime;
            M2Share.WorldEngine.SendBroadCastMsg("神之祈祷，天地震怒，尸横遍野...", MsgType.System);
            SysMsg("祈祷发出强烈的宇宙效应", MsgColor.Green, MsgType.Hint);
        }

        private void LogonTimcCost()
        {
            int n08;
            if (m_nPayMent == 2 || M2Share.Config.boTestServer)
            {
                n08 = (HUtil32.GetTickCount() - m_dwLogonTick) / 1000;
            }
            else
            {
                n08 = 0;
            }
            var sC = m_sIPaddr + "\t" + m_sUserID + "\t" + CharName + "\t" + n08 + "\t" + m_dLogonTime.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + m_nPayMode;
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
                int nOldX = CurrX;
                int nOldY = CurrY;
                Direction = btDir;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true) > 0)
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true) > 0)
                        {
                            CurrX += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true) > 0)
                        {
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true) > 0)
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true) > 0)
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (CurrX > 1 && CurrY > 1 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true) > 0)
                        {
                            CurrX -= 2;
                            CurrY -= 2;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Grobal2.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = (short)nOldX;
                        CurrY = (short)nOldY;
                        Envir.MoveToMovingObject(nOldX, nOldY, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
            return result;
        }

        private bool HorseRunTo(byte btDir, bool boFlag)
        {
            const string sExceptionMsg = "[Exception] TPlayObject::HorseRunTo";
            var result = false;
            try
            {
                int n10 = CurrX;
                int n14 = CurrY;
                Direction = btDir;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (CurrY > 2 && Envir.CanWalkEx(CurrX, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY - 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 3, true) > 0)
                        {
                            CurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (CurrX < Envir.Width - 3 && CurrY > 2 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 3, CurrY - 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY - 3, true) > 0)
                        {
                            CurrX += 3;
                            CurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (CurrX < Envir.Width - 3 && Envir.CanWalkEx(CurrX + 1, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 3, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY, true) > 0)
                        {
                            CurrX += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (CurrX < Envir.Width - 3 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX + 3, CurrY + 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY + 3, true) > 0)
                        {
                            CurrX += 3;
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX, CurrY + 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 3, true) > 0)
                        {
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (CurrX > 2 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 3, CurrY + 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY + 3, true) > 0)
                        {
                            CurrX -= 3;
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (CurrX > 2 && Envir.CanWalkEx(CurrX - 1, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 3, CurrY, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY, true) > 0)
                        {
                            CurrX -= 3;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (CurrX > 2 && CurrY > 2 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.CanWalkEx(CurrX - 3, CurrY - 3, M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY - 3, true) > 0)
                        {
                            CurrX -= 3;
                            CurrY -= 3;
                        }
                        break;
                }
                if (CurrX != n10 || CurrY != n14)
                {
                    if (Walk(Grobal2.RM_HORSERUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = (short)n10;
                        CurrY = (short)n14;
                        Envir.MoveToMovingObject(n10, n14, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
            return result;
        }

        protected void ThrustingOnOff(bool boSwitch)
        {
            UseThrusting = boSwitch;
            if (UseThrusting)
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
            UseHalfMoon = boSwitch;
            if (UseHalfMoon)
            {
                SysMsg(M2Share.sHalfMoonOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(M2Share.sHalfMoonOff, MsgColor.Green, MsgType.Hint);
            }
        }

        private void RedHalfMoonOnOff(bool boSwitch)
        {
            RedUseHalfMoon = boSwitch;
            if (RedUseHalfMoon)
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
            CrsHitkill = boSwitch;
            if (CrsHitkill)
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
            MBoTwinHitSkill = boSwitch;
            if (MBoTwinHitSkill)
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
            MBo43Kill = boSwitch;
            if (MBo43Kill)
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
            if (HUtil32.GetTickCount() - MDwLatestFireHitTick > 10 * 1000)
            {
                MDwLatestFireHitTick = HUtil32.GetTickCount();
                FireHitSkill = true;
                SysMsg(M2Share.sFireSpiritsSummoned, MsgColor.Green, MsgType.Hint);
                return true;
            }
            SysMsg(M2Share.sFireSpiritsFail, MsgColor.Red, MsgType.Hint);
            return false;
        }

        private bool AllowTwinHitSkill()
        {
            MDwLatestTwinHitTick = HUtil32.GetTickCount();
            MBoTwinHitSkill = true;
            SysMsg("twin hit skill charged", MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void ClientClickNpc(int actorId)
        {
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            if (Death || Ghost)
            {
                return;
            }
            if (HUtil32.GetTickCount() - m_dwClickNpcTime > M2Share.Config.ClickNpcTime)
            {
                m_dwClickNpcTime = HUtil32.GetTickCount();
                var normNpc = (NormNpc)M2Share.WorldEngine.FindMerchant(actorId) ?? (NormNpc)M2Share.WorldEngine.FindNpc(actorId);
                if (normNpc != null)
                {
                    if (normNpc.Envir == Envir && Math.Abs(normNpc.CurrX - CurrX) <= 15 && Math.Abs(normNpc.CurrY - CurrY) <= 15)
                    {
                        normNpc.Click(this);
                    }
                }
            }
        }

        private int GetRangeHumanCount()
        {
            return M2Share.WorldEngine.GetMapOfRangeHumanCount(Envir, CurrX, CurrY, 10);
        }

        private void GetStartPoint()
        {
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        HomeMap = M2Share.StartPointList[i].m_sMapName;
                        HomeX = M2Share.StartPointList[i].m_nCurrX;
                        HomeY = M2Share.StartPointList[i].m_nCurrY;
                    }
                }
            }
            if (PvpLevel() >= 2)
            {
                HomeMap = M2Share.Config.RedHomeMap;
                HomeX = M2Share.Config.RedHomeX;
                HomeY = M2Share.Config.RedHomeY;
            }
        }

        private void MobPlace(string sX, string sY, string sMonName, string sCount)
        {

        }

        private void DealCancel()
        {
            if (!Dealing)
            {
                return;
            }
            Dealing = false;
            SendDefMessage(Grobal2.SM_DEALCANCEL, 0, 0, 0, 0, "");
            if (DealCreat != null)
            {
                (DealCreat as PlayObject).DealCancel();
            }
            DealCreat = null;
            GetBackDealItems();
            SysMsg(M2Share.g_sDealActionCancelMsg, MsgColor.Green, MsgType.Hint);
            DealLastTick = HUtil32.GetTickCount();
        }

        public void DealCancelA()
        {
            Abil.HP = WAbil.HP;
            DealCancel();
        }

        public bool DecGold(int nGold)
        {
            if (Gold >= nGold)
            {
                Gold -= nGold;
                return true;
            }
            return false;
        }

        public void GainExp(int dwExp)
        {
            int n;
            int sumlv;
            PlayObject PlayObject;
            const string sExceptionMsg = "[Exception] TPlayObject::GainExp";
            double[] bonus = { 1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2 };
            try
            {
                if (GroupOwner != null)
                {
                    sumlv = 0;
                    n = 0;
                    for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
                    {
                        PlayObject = GroupOwner.GroupMembers[i];
                        if (!PlayObject.Death && Envir == PlayObject.Envir && Math.Abs(CurrX - PlayObject.CurrX) <= 12 && Math.Abs(CurrX - PlayObject.CurrX) <= 12)
                        {
                            sumlv = sumlv + PlayObject.Abil.Level;
                            n++;
                        }
                    }
                    if (sumlv > 0 && n > 1)
                    {
                        if (n >= 0 && n <= Grobal2.GROUPMAX)
                        {
                            dwExp = HUtil32.Round(dwExp * bonus[n]);
                        }
                        for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
                        {
                            PlayObject = GroupOwner.GroupMembers[i];
                            if (!PlayObject.Death && Envir == PlayObject.Envir && Math.Abs(CurrX - PlayObject.CurrX) <= 12 && Math.Abs(CurrX - PlayObject.CurrX) <= 12)
                            {
                                if (M2Share.Config.HighLevelKillMonFixExp)
                                {
                                    PlayObject.WinExp(HUtil32.Round(dwExp / n)); // 在高等级经验不变时，把组队的经验平均分配
                                }
                                else
                                {
                                    PlayObject.WinExp(HUtil32.Round(dwExp / sumlv * PlayObject.Abil.Level));
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
                M2Share.Log.Error(sExceptionMsg);
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

        private void GetBackDealItems()
        {
            if (DealItemList.Count > 0)
            {
                for (var i = 0; i < DealItemList.Count; i++)
                {
                    ItemList.Add(DealItemList[i]);
                }
            }
            DealItemList.Clear();
            Gold += DealGolds;
            DealGolds = 0;
            DealSuccess = false;
        }

        public override string GeTBaseObjectInfo()
        {
            return this.CharName + " 标识:" + this.ActorId + " 权限等级: " + this.Permission + " 管理模式: " + HUtil32.BoolToStr(this.AdminMode)
                + " 隐身模式: " + HUtil32.BoolToStr(this.ObMode) + " 无敌模式: " + HUtil32.BoolToStr(this.SuperMan) + " 地图:" + this.MapName + '(' + this.Envir.MapDesc + ')'
                + " 座标:" + this.CurrX + ':' + this.CurrY + " 等级:" + this.Abil.Level + " 转生等级:" + m_btReLevel
                + " 经验:" + this.Abil.Exp + " 生命值: " + this.Abil.HP + '-' + this.Abil.MaxHP + " 魔法值: " + this.Abil.MP + '-' + this.Abil.MaxMP
                + " 攻击力: " + HUtil32.LoWord(this.Abil.DC) + '-' + HUtil32.HiWord(this.Abil.DC) + " 魔法力: " + HUtil32.LoWord(this.Abil.MC) + '-'
                + HUtil32.HiWord(this.Abil.MC) + " 道术: " + HUtil32.LoWord(this.Abil.SC) + '-' + HUtil32.HiWord(this.Abil.SC)
                + " 防御力: " + HUtil32.LoWord(this.Abil.AC) + '-' + HUtil32.HiWord(this.Abil.AC) + " 魔防力: " + HUtil32.LoWord(this.Abil.MAC)
                + '-' + HUtil32.HiWord(this.Abil.MAC) + " 准确:" + this.HitPoint + " 敏捷:" + this.SpeedPoint + " 速度:" + this.HitSpeed
                + " 仓库密码:" + m_sStoragePwd + " 登录IP:" + m_sIPaddr + '(' + m_sIPLocal + ')' + " 登录帐号:" + m_sUserID + " 登录时间:" + m_dLogonTime
                + " 在线时长(分钟):" + (HUtil32.GetTickCount() - m_dwLogonTick) / 60000 + " 登录模式:" + m_nPayMent + ' ' + M2Share.Config.GameGoldName + ':' + m_nGameGold
                + ' ' + M2Share.Config.GamePointName + ':' + m_nGamePoint + ' ' + M2Share.Config.PayMentPointName + ':' + m_nPayMentPoint + " 会员类型:" + m_nMemberType
                + " 会员等级:" + m_nMemberLevel + " 经验倍数:" + m_nKillMonExpRate / 100 + " 攻击倍数:" + m_nPowerRate / 100 + " 声望值:" + m_btCreditPoint;
        }

        private int GetDigUpMsgCount()
        {
            var result = 0;
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < MsgList.Count; i++)
                {
                    SendMessage = MsgList[i];
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
                M2Share.AddGameDataLog(s10 + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + sChrName);
            }
        }

        public void ClearStatusTime()
        {
            this.StatusTimeArr = new ushort[12];
        }

        private void SendMapDescription()
        {
            var nMUSICID = -1;
            if (Envir.Flag.boMUSIC)
            {
                nMUSICID = Envir.Flag.nMUSICID;
            }
            SendDefMessage(Grobal2.SM_MAPDESCRIPTION, nMUSICID, 0, 0, 0, Envir.MapDesc);
        }

        private void SendWhisperMsg(PlayObject PlayObject)
        {
            if (PlayObject == this)
            {
                return;
            }
            if (PlayObject.Permission >= 9 || Permission >= 9)
            {
                return;
            }
            if (M2Share.WorldEngine.PlayObjectCount < M2Share.Config.nSendWhisperPlayCount + M2Share.RandomNumber.Random(5))
            {
                return;
            }
        }

        private void ReadAllBook()
        {
            for (var i = 0; i < M2Share.WorldEngine.MagicList.Count; i++)
            {
                var Magic = M2Share.WorldEngine.MagicList[i];
                var UserMagic = new UserMagic
                {
                    MagicInfo = Magic,
                    wMagIdx = Magic.wMagicID,
                    btLevel = 2,
                    btKey = 0
                };
                UserMagic.btLevel = 0;
                UserMagic.nTranPoint = 100000;
                MagicList.Add(UserMagic);
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
                sMsg = M2Share.Config.GameGoldName + '\r' + M2Share.Config.GamePointName;
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
            if (M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll)
            {
                nRunHuman = 1;
                nRunMon = 1;
                nRunNpc = 1;
                nWarRunAll = 1;
            }
            else
            {
                if (M2Share.Config.boRunHuman || Envir.Flag.boRUNHUMAN)
                {
                    nRunHuman = 1;
                }
                if (M2Share.Config.boRunMon || Envir.Flag.boRUNMON)
                {
                    nRunMon = 1;
                }
                if (M2Share.Config.boRunNpc)
                {
                    nRunNpc = 1;
                }
                if (M2Share.Config.boWarDisHumRun)
                {
                    nWarRunAll = 1;
                }
            }
            var ClientConf = M2Share.Config.ClientConf;
            ClientConf.boRunHuman = nRunHuman == 1;
            ClientConf.boRunMon = nRunMon == 1;
            ClientConf.boRunNpc = nRunNpc == 1;
            ClientConf.boWarRunAll = nWarRunAll == 1;
            ClientConf.wSpellTime = (ushort)(M2Share.Config.MagicHitIntervalTime + 300);
            ClientConf.wHitIime = (ushort)(M2Share.Config.HitIntervalTime + 500);
            var sMsg = EDCode.EncodeBuffer(ClientConf);
            var nRecog = HUtil32.MakeLong(HUtil32.MakeWord(nRunHuman, nRunMon), HUtil32.MakeWord(nRunNpc, nWarRunAll));
            var nParam = (short)HUtil32.MakeWord(5, 0);
            SendDefMessage(Grobal2.SM_SERVERCONFIG, nRecog, nParam, 0, 0, sMsg);
        }

        private void SendServerStatus()
        {
            if (Permission < 10)
            {
                return;
            }
            //this.SysMsg((HUtil32.CalcFileCRC(Application.ExeName)).ToString(), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        // 检查角色的座标是否在指定误差范围以内
        // TargeTBaseObject 为要检查的角色，nX,nY 为比较的座标
        // 检查角色是否在指定座标的1x1 范围以内，如果在则返回True 否则返回 False
        protected bool CretInNearXY(BaseObject TargeTBaseObject, int nX, int nY)
        {
            MapCellInfo cellInfo;
            CellObject OSObject;
            BaseObject BaseObject;
            if (Envir == null)
            {
                M2Share.Log.Error("CretInNearXY nil PEnvir");
                return false;
            }
            for (var nCX = nX - 1; nCX <= nX + 1; nCX++)
            {
                for (var nCY = nY - 1; nCY <= nY + 1; nCY++)
                {
                    var cellsuccess = false;
                    cellInfo = Envir.GetCellInfo(nCX, nCY, ref cellsuccess);
                    if (cellsuccess && cellInfo.IsAvailable)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            OSObject = cellInfo.ObjList[i];
                            if (OSObject.CellType == CellType.MovingObject)
                            {
                                BaseObject = M2Share.ActorMgr.Get(OSObject.CellObjId); ;
                                if (BaseObject != null)
                                {
                                    if (!BaseObject.Ghost && BaseObject == TargeTBaseObject)
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
            var sSendMsg = string.Empty;
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].wIndex > 0)
                {
                    var Item = M2Share.WorldEngine.GetStdItem(UseItems[i].wIndex);
                    if (Item != null)
                    {
                        var ClientItem = new ClientItem();
                        Item.GetStandardItem(ref ClientItem.Item);
                        Item.GetItemAddValue(UseItems[i], ref ClientItem.Item);
                        ClientItem.Item.Name = ItemUnit.GetItemName(UseItems[i]);
                        ClientItem.Dura = UseItems[i].Dura;
                        ClientItem.DuraMax = UseItems[i].DuraMax;
                        ClientItem.MakeIndex = UseItems[i].MakeIndex;
                        sSendMsg = sSendMsg + i + '/' + EDCode.EncodeBuffer(ClientItem) + '/';
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
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                var clientMagic = new ClientMagic();
                clientMagic.Key = (char)userMagic.btKey;
                clientMagic.Level = userMagic.btLevel;
                clientMagic.CurTrain = userMagic.nTranPoint;
                clientMagic.Def = userMagic.MagicInfo;
                sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientMagic) + '/';
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDMYMAGIC, 0, 0, 0, (short)MagicList.Count);
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

        private void RecalcAdjusBonusAdjustAb(byte abil, short val, ref short lov, ref short hiv)
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
            NakedAbility BonusTick = null;
            NakedAbility NakedAbil = null;
            switch (Job)
            {
                case PlayJob.Warrior:
                    BonusTick = M2Share.Config.BonusAbilofWarr;
                    NakedAbil = M2Share.Config.NakedAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    BonusTick = M2Share.Config.BonusAbilofWizard;
                    NakedAbil = M2Share.Config.NakedAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    BonusTick = M2Share.Config.BonusAbilofTaos;
                    NakedAbil = M2Share.Config.NakedAbilofTaos;
                    break;
            }
            var adc = (short)(BonusAbil.DC / BonusTick.DC);
            var amc = (short)(BonusAbil.MC / BonusTick.MC);
            var asc = (short)(BonusAbil.SC / BonusTick.SC);
            var aac = (short)(BonusAbil.AC / BonusTick.AC);
            var amac = (short)(BonusAbil.MAC / BonusTick.MAC);
            RecalcAdjusBonusAdjustAb((byte)NakedAbil.DC, adc, ref ldc, ref hdc);
            RecalcAdjusBonusAdjustAb((byte)NakedAbil.MC, amc, ref lmc, ref hmc);
            RecalcAdjusBonusAdjustAb((byte)NakedAbil.SC, asc, ref lsc, ref hsc);
            RecalcAdjusBonusAdjustAb((byte)NakedAbil.AC, aac, ref lac, ref hac);
            RecalcAdjusBonusAdjustAb((byte)NakedAbil.MAC, amac, ref lmac, ref hmac);
            Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) + ldc, HUtil32.HiWord(Abil.DC) + hdc);
            Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MC) + lmc, HUtil32.HiWord(Abil.MC) + hmc);
            Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(Abil.SC) + lsc, HUtil32.HiWord(Abil.SC) + hsc);
            Abil.AC = HUtil32.MakeLong(HUtil32.LoWord(Abil.AC) + lac, HUtil32.HiWord(Abil.AC) + hac);
            Abil.MAC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MAC) + lmac, HUtil32.HiWord(Abil.MAC) + hmac);
            Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxHP + BonusAbil.HP / BonusTick.HP);
            Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, Abil.MaxMP + BonusAbil.MP / BonusTick.MP);
        }

        private void ClientAdjustBonus(int nPoint, string sMsg)
        {
            var BonusAbil = new NakedAbility();
            var nTotleUsePoint = BonusAbil.DC + BonusAbil.MC + BonusAbil.SC + BonusAbil.AC + BonusAbil.MAC + BonusAbil.HP + BonusAbil.MP + BonusAbil.Hit + BonusAbil.Speed + BonusAbil.X2;
            if (nPoint + nTotleUsePoint == BonusPoint)
            {
                BonusPoint = nPoint;
                this.BonusAbil.DC += BonusAbil.DC;
                this.BonusAbil.MC += BonusAbil.MC;
                this.BonusAbil.SC += BonusAbil.SC;
                this.BonusAbil.AC += BonusAbil.AC;
                this.BonusAbil.MAC += BonusAbil.MAC;
                this.BonusAbil.HP += BonusAbil.HP;
                this.BonusAbil.MP += BonusAbil.MP;
                this.BonusAbil.Hit += BonusAbil.Hit;
                this.BonusAbil.Speed += BonusAbil.Speed;
                this.BonusAbil.X2 += BonusAbil.X2;
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
            var result = HungerStatus / 1000;
            if (result > 4)
            {
                result = 4;
            }
            return result;
        }

        private void SendAdjustBonus()
        {
            var sSendMsg = string.Empty;
            switch (Job)
            {
                case PlayJob.Warrior:
                    sSendMsg = EDCode.EncodeBuffer(M2Share.Config.BonusAbilofWarr) + '/' + EDCode.EncodeBuffer(BonusAbil) + '/' + EDCode.EncodeBuffer(M2Share.Config.NakedAbilofWarr);
                    break;
                case PlayJob.Wizard:
                    sSendMsg = EDCode.EncodeBuffer(M2Share.Config.BonusAbilofWizard) + '/' + EDCode.EncodeBuffer(BonusAbil) + '/' + EDCode.EncodeBuffer(M2Share.Config.NakedAbilofWizard);
                    break;
                case PlayJob.Taoist:
                    sSendMsg = EDCode.EncodeBuffer(M2Share.Config.BonusAbilofTaos) + '/' + EDCode.EncodeBuffer(BonusAbil) + '/' + EDCode.EncodeBuffer(M2Share.Config.NakedAbilofTaos);
                    break;
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADJUST_BONUS, BonusPoint, 0, 0, 0);
            SendSocket(m_DefMsg, sSendMsg);
        }

        public void PKDie(PlayObject PlayObject)
        {
            var nWinLevel = M2Share.Config.KillHumanWinLevel;
            var nLostLevel = M2Share.Config.KilledLostLevel;
            var nWinExp = M2Share.Config.KillHumanWinExp;
            var nLostExp = M2Share.Config.KillHumanLostExp;
            var boWinLEvel = M2Share.Config.IsKillHumanWinLevel;
            var boLostLevel = M2Share.Config.IsKilledLostLevel;
            var boWinExp = M2Share.Config.IsKillHumanWinExp;
            var boLostExp = M2Share.Config.IsKilledLostExp;
            if (Envir.Flag.boPKWINLEVEL)
            {
                boWinLEvel = true;
                nWinLevel = Envir.Flag.nPKWINLEVEL;
            }
            if (Envir.Flag.boPKLOSTLEVEL)
            {
                boLostLevel = true;
                nLostLevel = Envir.Flag.nPKLOSTLEVEL;
            }
            if (Envir.Flag.boPKWINEXP)
            {
                boWinExp = true;
                nWinExp = Envir.Flag.nPKWINEXP;
            }
            if (Envir.Flag.boPKLOSTEXP)
            {
                boLostExp = true;
                nLostExp = Envir.Flag.nPKLOSTEXP;
            }
            if (PlayObject.Abil.Level - Abil.Level > M2Share.Config.HumanLevelDiffer)
            {
                if (!PlayObject.IsGoodKilling(this))
                {
                    PlayObject.IncPkPoint(M2Share.Config.KillHumanAddPKPoint);
                    PlayObject.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                    SysMsg(Format(M2Share.g_sYouKilledByMsg, LastHiter.CharName), MsgColor.Red, MsgType.Hint);
                    PlayObject.AddBodyLuck(-M2Share.Config.KillHumanDecLuckPoint);
                    if (PvpLevel() < 1)
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
                    PlayObject.SysMsg(M2Share.g_sYouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                }
                return;
            }
            if (boWinLEvel)
            {
                if (PlayObject.Abil.Level + nWinLevel <= M2Share.MAXUPLEVEL)
                {
                    PlayObject.Abil.Level += (byte)nWinLevel;
                }
                else
                {
                    PlayObject.Abil.Level = M2Share.MAXUPLEVEL;
                }
                PlayObject.HasLevelUp(PlayObject.Abil.Level - nWinLevel);
                if (boLostLevel)
                {
                    if (PvpLevel() >= 2)
                    {
                        if (Abil.Level >= nLostLevel * 2)
                        {
                            Abil.Level -= (byte)(nLostLevel * 2);
                        }
                    }
                    else
                    {
                        if (Abil.Level >= nLostLevel)
                        {
                            Abil.Level -= (byte)nLostLevel;
                        }
                    }
                }
            }
            if (boWinExp)
            {
                PlayObject.WinExp(nWinExp);
                if (boLostExp)
                {
                    if (Abil.Exp >= nLostExp)
                    {
                        if (Abil.Exp >= nLostExp)
                        {
                            Abil.Exp -= nLostExp;
                        }
                        else
                        {
                            Abil.Exp = 0;
                        }
                    }
                    else
                    {
                        if (Abil.Level >= 1)
                        {
                            Abil.Level -= 1;
                            Abil.Exp += GetLevelExp(Abil.Level);
                            if (Abil.Exp >= nLostExp)
                            {
                                Abil.Exp -= nLostExp;
                            }
                            else
                            {
                                Abil.Exp = 0;
                            }
                        }
                        else
                        {
                            Abil.Level = 0;
                            Abil.Exp = 0;
                        }
                    }
                }
            }
        }

        public bool CancelGroup()
        {
            var result = true;
            const string sCanceGrop = "你的小组被解散了.";
            if (GroupMembers.Count <= 1)
            {
                SendGroupText(sCanceGrop);
                GroupMembers.Clear();
                GroupOwner = null;
                result = false;
            }
            return result;
        }

        public void SendGroupMembers()
        {
            PlayObject PlayObject;
            var sSendMsg = "";
            for (var i = 0; i < GroupMembers.Count; i++)
            {
                PlayObject = GroupMembers[i];
                sSendMsg = sSendMsg + PlayObject.CharName + '/';
            }
            for (var i = 0; i < GroupMembers.Count; i++)
            {
                PlayObject = GroupMembers[i];
                PlayObject.SendDefMessage(Grobal2.SM_GROUPMEMBERS, 0, 0, 0, 0, sSendMsg);
            }
        }

        protected ushort GetSpellPoint(UserMagic UserMagic)
        {
            return (ushort)(HUtil32.Round(UserMagic.MagicInfo.wSpell / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefSpell);
        }

        private bool DoMotaeboCanMotaebo(BaseObject BaseObject, int nMagicLevel)
        {
            if (Abil.Level > BaseObject.Abil.Level && !BaseObject.StickMode) //当前玩家等级大于目标等级，并且目标可以被冲撞
            {
                var nC = Abil.Level - BaseObject.Abil.Level;
                if (M2Share.RandomNumber.Random(20) < nMagicLevel * 4 + 6 + nC)
                {
                    if (IsProperTarget(BaseObject))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool DoMotaebo(byte nDir, int nMagicLevel)
        {
            int nDmg;
            BaseObject BaseObject_30 = null;
            BaseObject BaseObject_34 = null;
            short nX = 0;
            short nY = 0;
            var result = false;
            var bo35 = true;
            var n24 = nMagicLevel + 1;
            var n28 = n24;
            Direction = nDir;
            var PoseCreate = GetPoseCreate();
            if (PoseCreate != null)
            {
                for (var i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    PoseCreate = GetPoseCreate();
                    if (PoseCreate != null)
                    {
                        n28 = 0;
                        if (!DoMotaeboCanMotaebo(PoseCreate, nMagicLevel))
                        {
                            break;
                        }
                        if (nMagicLevel >= 3)
                        {
                            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref nX, ref nY))
                            {
                                BaseObject_30 = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                                if (BaseObject_30 != null && DoMotaeboCanMotaebo(BaseObject_30, nMagicLevel))
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
                        if (Envir.MoveToMovingObject(CurrX, CurrY, this, nX, nY, false) > 0)
                        {
                            CurrX = nX;
                            CurrY = nY;
                            SendRefMsg(Grobal2.RM_RUSH, nDir, CurrX, CurrY, 0, "");
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
                    if (Envir.MoveToMovingObject(CurrX, CurrY, this, nX, nY, false) > 0)
                    {
                        CurrX = nX;
                        CurrY = nY;
                        SendRefMsg(Grobal2.RM_RUSH, nDir, CurrX, CurrY, 0, "");
                        n28 -= 1;
                    }
                    else
                    {
                        if (Envir.CanWalk(nX, nY, true))
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
                BaseObject_34.SendRefMsg(Grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.Abil.HP, BaseObject_34.Abil.MaxHP, ActorId, "");
                if (BaseObject_34.Race != Grobal2.RC_PLAYOBJECT)
                {
                    BaseObject_34.SendMsg(BaseObject_34, Grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.Abil.HP, BaseObject_34.Abil.MaxHP, ActorId, "");
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
                SendRefMsg(Grobal2.RM_STRUCK, (short)nDmg, Abil.HP, Abil.MaxHP, 0, "");
            }
            return result;
        }

        private bool DoSpell(UserMagic UserMagic, short nTargetX, short nTargetY, BaseObject BaseObject)
        {
            var result = false;
            try
            {
                if (!M2Share.MagicMgr.IsWarrSkill(UserMagic.wMagIdx))
                {
                    var nSpellPoint = GetSpellPoint(UserMagic);
                    if (nSpellPoint > 0)
                    {
                        if (Abil.MP < nSpellPoint)
                        {
                            return result;
                        }
                        DamageSpell(nSpellPoint);
                        HealthSpellChanged();
                    }
                    result = M2Share.MagicMgr.DoSpell(this, UserMagic, nTargetX, nTargetY, BaseObject);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(Format("[Exception] TPlayObject.DoSpell MagID:{0} X:{1} Y:{2}", UserMagic.wMagIdx, nTargetX, nTargetY));
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 挖矿
        /// </summary>
        /// <returns></returns>
        private bool PileStones(int nX, int nY)
        {
            var result = false;
            var s1C = string.Empty;
            var mineEvent = (StoneMineEvent)Envir.GetEvent(nX, nY);
            if (mineEvent != null && mineEvent.EventType == Grobal2.ET_MINE)
            {
                if (mineEvent.MineCount > 0)
                {
                    mineEvent.MineCount -= 1;
                    if (M2Share.RandomNumber.Random(M2Share.Config.MakeMineHitRate) == 0)
                    {
                        var pileEvent = (PileStones)Envir.GetEvent(CurrX, CurrY);
                        if (pileEvent == null)
                        {
                            pileEvent = new PileStones(Envir, CurrX, CurrY, Grobal2.ET_PILESTONES, 5 * 60 * 1000);
                            M2Share.EventMgr.AddEvent(pileEvent);
                        }
                        else
                        {
                            if (pileEvent.EventType == Grobal2.ET_PILESTONES)
                            {
                                pileEvent.AddEventParam();
                            }
                        }
                        if (M2Share.RandomNumber.Random(M2Share.Config.MakeMineRate) == 0)
                        {
                            if (Envir.Flag.boMINE)
                            {
                                MakeMine();
                            }
                            else if (Envir.Flag.boMINE2)
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
                    if (HUtil32.GetTickCount() - mineEvent.AddStoneMineTick > 10 * 60 * 1000)
                    {
                        mineEvent.AddStoneMine();
                    }
                }
            }
            SendRefMsg(Grobal2.RM_HEAVYHIT, Direction, CurrX, CurrY, 0, s1C);
            return result;
        }

        private void SendSaveItemList(int nBaseObject)
        {
            var sSendMsg = string.Empty;
            for (var i = 0; i < StorageItemList.Count; i++)
            {
                var UserItem = StorageItemList[i];
                var Item = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
                if (Item != null)
                {
                    ClientItem ClientItem = null;
                    Item.GetStandardItem(ref ClientItem.Item);
                    Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                    ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    sSendMsg = sSendMsg + EDCode.EncodeBuffer(ClientItem) + '/';
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SAVEITEMLIST, nBaseObject, 0, 0, (short)StorageItemList.Count);
            SendSocket(m_DefMsg, sSendMsg);
        }

        private void SendChangeGuildName()
        {
            if (MyGuild != null)
            {
                SendDefMessage(Grobal2.SM_CHANGEGUILDNAME, 0, 0, 0, 0, MyGuild.sGuildName + '/' + GuildRankName);
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
            SendSocket(m_DefMsg, EDCode.EncodeString(s10));
        }

        public void SendDelItems(UserItem UserItem)
        {
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                var ClientItem = new ClientItem();
                StdItem.GetStandardItem(ref ClientItem.Item);
                StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
                ClientItem.MakeIndex = UserItem.MakeIndex;
                ClientItem.Dura = UserItem.Dura;
                ClientItem.DuraMax = UserItem.DuraMax;
                if (StdItem.StdMode == 50)
                {
                    ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELITEM, ActorId, 0, 0, 1);
                SendSocket(m_DefMsg, EDCode.EncodeBuffer(ClientItem));
            }
        }

        public void SendUpdateItem(UserItem UserItem)
        {
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                var ClientItem = new ClientItem();
                StdItem.GetStandardItem(ref ClientItem.Item);
                StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
                ClientItem.MakeIndex = UserItem.MakeIndex;
                ClientItem.Dura = UserItem.Dura;
                ClientItem.DuraMax = UserItem.DuraMax;
                if (StdItem.StdMode == 50)
                {
                    ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(m_DefMsg, EDCode.EncodeBuffer(ClientItem));
            }
        }

        private bool CheckTakeOnItems(int nWhere, ref ClientStdItem StdItem)
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
                if (StdItem.Weight > Abil.MaxHandWeight)
                {
                    SysMsg(M2Share.sHandWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            else
            {
                if (StdItem.Weight + GetUserItemWeitht(nWhere) > Abil.MaxWearWeight)
                {
                    SysMsg(M2Share.sWearWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            switch (StdItem.Need)
            {
                case 0:
                    if (Abil.Level >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 1:
                    if (HUtil32.HiWord(Abil.DC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 10:
                    if (Job == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 11:
                    if (Job == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(Abil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 12:
                    if (Job == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(Abil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 13:
                    if (Job == (PlayJob)HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(Abil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 2:
                    if (HUtil32.HiWord(Abil.MC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 3:
                    if (HUtil32.HiWord(Abil.SC) >= StdItem.NeedLevel)
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
                        if (Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
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
                        if (HUtil32.HiWord(Abil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
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
                        if (HUtil32.HiWord(Abil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
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
                        if (HUtil32.HiWord(Abil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
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
                    if (MyGuild != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 60:
                    if (MyGuild != null && GuildRankNo == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildMasterNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 7:
                    if (MyGuild != null && M2Share.CastleMgr.IsCastleMember(this) != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSabukHumanNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 70:
                    if (MyGuild != null && M2Share.CastleMgr.IsCastleMember(this) != null && GuildRankNo == 1)
                    {
                        if (Abil.Level >= StdItem.NeedLevel)
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
            var n14 = 0;
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (nWhere == -1 || !(i == nWhere) && !(i == 1) && !(i == 2))
                {
                    if (UseItems[i] == null)
                    {
                        continue;
                    }
                    var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        n14 += StdItem.Weight;
                    }
                }
            }
            return n14;
        }

        private bool EatItems(StdItem StdItem, UserItem Useritem)
        {
            var result = false;
            if (Envir.Flag.boNODRUG)
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
                            UserUnLockDurg = true;
                            result = true;
                            break;
                        case 3:
                            IncHealthSpell(HUtil32.Round(Abil.MaxHP / 100 * StdItem.Ac), HUtil32.Round(Abil.MaxMP / 100 * StdItem.Mac));
                            result = true;
                            break;
                        default:
                            if (StdItem.Ac > 0)
                            {
                                IncHealth += StdItem.Ac;
                            }
                            if (StdItem.Mac > 0)
                            {
                                IncSpell += StdItem.Mac;
                            }
                            result = true;
                            break;
                    }
                    break;
                case 1:
                    var nOldStatus = GetMyStatus();
                    HungerStatus += StdItem.DuraMax / 10;
                    HungerStatus = HUtil32._MIN(5000, HungerStatus);
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
                                StatusArrValue[0] = StdItem.Dc;
                                StatusArrTimeOutTick[0] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("攻击力增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Mc > 0)
                            {
                                StatusArrValue[1] = StdItem.Mc;
                                StatusArrTimeOutTick[1] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("魔法力增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Sc > 0)
                            {
                                StatusArrValue[2] = StdItem.Sc;
                                StatusArrTimeOutTick[2] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("道术增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Ac2 > 0)
                            {
                                StatusArrValue[3] = StdItem.Ac2;
                                StatusArrTimeOutTick[3] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("攻击速度增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Ac > 0)
                            {
                                StatusArrValue[4] = StdItem.Ac;
                                StatusArrTimeOutTick[4] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
                                SysMsg("生命值增加" + StdItem.Mac2 + "秒.", MsgColor.Green, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.Mac > 0)
                            {
                                StatusArrValue[5] = StdItem.Mac;
                                StatusArrTimeOutTick[5] = HUtil32.GetTickCount() + StdItem.Mac2 * 1000;
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
            var result = false;
            var magic = M2Share.WorldEngine.FindMagic(StdItem.Name);
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.wMagicID))
                {
                    if (magic.btJob == 99 || magic.btJob == (byte)Job)
                    {
                        if (Abil.Level >= magic.TrainLevel[0])
                        {
                            var UserMagic = new UserMagic
                            {
                                MagicInfo = magic,
                                wMagIdx = magic.wMagicID,
                                btKey = 0,
                                btLevel = 0,
                                nTranPoint = 0
                            };
                            MagicList.Add(UserMagic);
                            RecalcAbilitys();
                            if (Race == Grobal2.RC_PLAYOBJECT)
                            {
                                this.SendAddMagic(UserMagic);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public void SendAddMagic(UserMagic UserMagic)
        {
            var clientMagic = new ClientMagic
            {
                Key = (char)UserMagic.btKey,
                Level = UserMagic.btLevel,
                CurTrain = UserMagic.nTranPoint,
                Def = UserMagic.MagicInfo
            };
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDMAGIC, 0, 0, 0, 1);
            SendSocket(m_DefMsg, EDCode.EncodeBuffer(clientMagic));
        }

        internal void SendDelMagic(UserMagic UserMagic)
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
                    BaseObjectMove(HomeMap, 0, 0);
                    result = true;
                    break;
                case 2:
                    if (!Envir.Flag.boNORANDOMMOVE)
                    {
                        SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        BaseObjectMove(MapName, 0, 0);
                        result = true;
                    }
                    break;
                case 3:
                    SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    if (PvpLevel() < 2)
                    {
                        BaseObjectMove(HomeMap, HomeX, HomeY);
                    }
                    else
                    {
                        BaseObjectMove(M2Share.Config.RedHomeMap, M2Share.Config.RedHomeX, M2Share.Config.RedHomeY);
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
                    if (MyGuild != null)
                    {
                        if (!InFreePkArea)
                        {
                            var Castle = M2Share.CastleMgr.IsCastleMember(this);
                            if (Castle != null && Castle.IsMasterGuild(MyGuild))
                            {
                                BaseObjectMove(Castle.HomeMap, Castle.GetHomeX(), Castle.GetHomeY());
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

        protected void MoveToHome()
        {
            SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            BaseObjectMove(HomeMap, HomeX, HomeY);
        }

        private void BaseObjectMove(string sMap, short sX, short sY)
        {
            if (string.IsNullOrEmpty(sMap))
            {
                sMap = MapName;
            }
            if (sX != 0 && sY != 0)
            {
                var nX = sX;
                var nY = sY;
                SpaceMove(sMap, nX, nY, 0);
            }
            else
            {
                MapRandomMove(sMap, 0);
            }
            var envir = Envir;
            if (envir != Envir && Race == Grobal2.RC_PLAYOBJECT)
            {
                m_boTimeRecall = false;
            }
        }

        private void ChangeServerMakeSlave(SlaveInfo slaveInfo)
        {
            var nSlavecount = 0;
            if (Job == PlayJob.Taoist)
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
                BaseObject.KillMonCount = slaveInfo.KillCount;
                BaseObject.SlaveExpLevel = slaveInfo.SlaveExpLevel;
                BaseObject.Abil.HP = slaveInfo.nHP;
                BaseObject.Abil.MP = slaveInfo.nMP;
                if (1500 - slaveInfo.SlaveLevel * 200 < BaseObject.WalkSpeed)
                {
                    BaseObject.WalkSpeed = (1500 - slaveInfo.SlaveLevel) * 200;
                }
                if (2000 - slaveInfo.SlaveLevel * 200 < BaseObject.NextHitTime)
                {
                    BaseObject.WalkSpeed = (2000 - slaveInfo.SlaveLevel) * 200;
                }
                RecalcAbilitys();
            }
        }

        private void SendDelDealItem(UserItem UserItem)
        {
            if (DealCreat != null)
            {
                var pStdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
                if (pStdItem != null)
                {
                    var ClientItem = new ClientItem();
                    pStdItem.GetStandardItem(ref ClientItem.Item);
                    ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEDELITEM, ActorId, 0, 0, 1);
                    (DealCreat as PlayObject)?.SendSocket(m_DefMsg, EDCode.EncodeBuffer(ClientItem));
                    DealCreat.DealLastTick = HUtil32.GetTickCount();
                    DealLastTick = HUtil32.GetTickCount();
                }
                SendDefMessage(Grobal2.SM_DEALDELITEM_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void SendAddDealItem(UserItem UserItem)
        {
            SendDefMessage(Grobal2.SM_DEALADDITEM_OK, 0, 0, 0, 0, "");
            if (DealCreat != null)
            {
                var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    var ClientItem = new ClientItem();
                    StdItem.GetStandardItem(ref ClientItem.Item);
                    StdItem.GetItemAddValue(UserItem, ref ClientItem.Item);
                    ClientItem.Item.Name = ItemUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEADDITEM, ActorId, 0, 0, 1);
                    (DealCreat as PlayObject).SendSocket(m_DefMsg, EDCode.EncodeBuffer(ClientItem));
                    DealCreat.DealLastTick = HUtil32.GetTickCount();
                    DealLastTick = HUtil32.GetTickCount();
                }
            }
        }

        private void OpenDealDlg(BaseObject BaseObject)
        {
            Dealing = true;
            DealCreat = BaseObject;
            GetBackDealItems();
            SendDefMessage(Grobal2.SM_DEALMENU, 0, 0, 0, 0, DealCreat.CharName);
            DealLastTick = HUtil32.GetTickCount();
        }

        private void JoinGroup(PlayObject PlayObject)
        {
            GroupOwner = PlayObject;
            SendGroupText(Format(M2Share.g_sJoinGroup, CharName));
        }

        /// <summary>
        /// 随机矿石持久度
        /// </summary>
        /// <returns></returns>
        private ushort MakeMineRandomDrua()
        {
            var result = M2Share.RandomNumber.Random(M2Share.Config.StoneGeneralDuraRate) + M2Share.Config.StoneMinDura;
            if (M2Share.RandomNumber.Random(M2Share.Config.StoneAddDuraRate) == 0)
            {
                result += M2Share.RandomNumber.Random(M2Share.Config.StoneAddDuraMax);
            }
            return (ushort)result;
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine()
        {
            UserItem UserItem = null;
            if (ItemList.Count >= Grobal2.MAXBAGITEM)
            {
                return;
            }
            var nRandom = M2Share.RandomNumber.Random(M2Share.Config.StoneTypeRate);
            if (nRandom >= M2Share.Config.GoldStoneMin && nRandom <= M2Share.Config.GoldStoneMax)
            {
                UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GoldStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.SilverStoneMin && nRandom <= M2Share.Config.SilverStoneMax)
            {
                UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.SilverStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.SteelStoneMin && nRandom <= M2Share.Config.SteelStoneMax)
            {
                UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.SteelStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.BlackStoneMin && nRandom <= M2Share.Config.BlackStoneMax)
            {
                UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.BlackStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            UserItem = new UserItem();
            if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.CopperStone, ref UserItem))
            {
                UserItem.Dura = MakeMineRandomDrua();
                ItemList.Add(UserItem);
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
            if (ItemList.Count >= Grobal2.MAXBAGITEM)
            {
                return;
            }
            UserItem mineItem = null;
            var mineRate = M2Share.RandomNumber.Random(120);
            if (HUtil32.RangeInDefined(mineRate, 1, 2))
            {
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GemStone1, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(mineItem);
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
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GemStone2, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(mineItem);
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
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GemStone3, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(mineItem);
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
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GemStone4, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(mineItem);
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
        public UserItem QuestCheckItem(string sItemName, ref int nCount, ref int nParam, ref int nDura)
        {
            string s1C;
            UserItem result = null;
            nParam = 0;
            nDura = 0;
            nCount = 0;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var UserItem = ItemList[i];
                s1C = M2Share.WorldEngine.GetStdItemName(UserItem.wIndex);
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

        public bool QuestTakeCheckItem(UserItem CheckItem)
        {
            var result = false;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var UserItem = ItemList[i];
                if (UserItem == CheckItem)
                {
                    SendDelItems(UserItem);
                    Dispose(UserItem);
                    ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == CheckItem)
                {
                    SendDelItems(UseItems[i]);
                    UseItems[i].wIndex = 0;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void MakeSaveRcd(ref THumDataInfo HumanRcd)
        {
            var HumData = HumanRcd.Data;
            HumData.sCharName = CharName;
            HumData.sCurMap = MapName;
            HumData.wCurX = CurrX;
            HumData.wCurY = CurrY;
            HumData.btDir = Direction;
            HumData.btHair = Hair;
            HumData.btSex = (byte)Gender;
            HumData.btJob = (byte)Job;
            HumData.nGold = Gold;
            HumData.Abil.Level = Abil.Level;
            HumData.Abil.HP = Abil.HP;
            HumData.Abil.MP = Abil.MP;
            HumData.Abil.MaxHP = Abil.MaxHP;
            HumData.Abil.MaxMP = Abil.MaxMP;
            HumData.Abil.Exp = Abil.Exp;
            HumData.Abil.MaxExp = Abil.MaxExp;
            HumData.Abil.Weight = Abil.Weight;
            HumData.Abil.MaxWeight = Abil.MaxWeight;
            HumData.Abil.WearWeight = Abil.WearWeight;
            HumData.Abil.MaxWearWeight = Abil.MaxWearWeight;
            HumData.Abil.HandWeight = Abil.HandWeight;
            HumData.Abil.MaxHandWeight = Abil.MaxHandWeight;
            HumData.Abil.HP = Abil.HP;
            HumData.Abil.MP = Abil.MP;
            HumData.wStatusTimeArr = StatusTimeArr;
            HumData.sHomeMap = HomeMap;
            HumData.wHomeX = HomeX;
            HumData.wHomeY = HomeY;
            HumData.nPKPoint = PkPoint;
            HumData.BonusAbil = BonusAbil;
            HumData.nBonusPoint = BonusPoint;
            HumData.sStoragePwd = m_sStoragePwd;
            HumData.btCreditPoint = m_btCreditPoint;
            HumData.btReLevel = m_btReLevel;
            HumData.sMasterName = m_sMasterName;
            HumData.boMaster = m_boMaster;
            HumData.sDearName = m_sDearName;
            HumData.nGameGold = m_nGameGold;
            HumData.nGamePoint = m_nGamePoint;
            if (AllowGroup)
            {
                HumData.btAllowGroup = 1;
            }
            else
            {
                HumData.btAllowGroup = 0;
            }
            HumData.btF9 = BtB2;
            HumData.btAttatckMode = (byte)AttatckMode;
            HumData.btIncHealth = (byte)IncHealth;
            HumData.btIncSpell = (byte)IncSpell;
            HumData.btIncHealing = (byte)IncHealing;
            HumData.btFightZoneDieCount = (byte)FightZoneDieCount;
            HumData.sAccount = m_sUserID;
            HumData.boLockLogon = m_boLockLogon;
            HumData.wContribution = m_wContribution;
            HumData.nHungerStatus = HungerStatus;
            HumData.boAllowGuildReCall = AllowGuildReCall;
            HumData.wGroupRcallTime = GroupRcallTime;
            HumData.dBodyLuck = BodyLuck;
            HumData.boAllowGroupReCall = AllowGroupReCall;
            HumData.QuestUnitOpen = QuestUnitOpen;
            HumData.QuestUnit = QuestUnit;
            HumData.QuestFlag = QuestFlag;
            var HumItems = HumanRcd.Data.HumItems;
            if (HumItems == null)
            {
                HumItems = new UserItem[13];
            }
            HumItems[Grobal2.U_DRESS] = UseItems[Grobal2.U_DRESS] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_DRESS];
            HumItems[Grobal2.U_WEAPON] = UseItems[Grobal2.U_WEAPON] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_WEAPON];
            HumItems[Grobal2.U_RIGHTHAND] = UseItems[Grobal2.U_RIGHTHAND] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_RIGHTHAND];
            HumItems[Grobal2.U_HELMET] = UseItems[Grobal2.U_NECKLACE] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_NECKLACE];
            HumItems[Grobal2.U_NECKLACE] = UseItems[Grobal2.U_HELMET] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_HELMET];
            HumItems[Grobal2.U_ARMRINGL] = UseItems[Grobal2.U_ARMRINGL] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_ARMRINGL];
            HumItems[Grobal2.U_ARMRINGR] = UseItems[Grobal2.U_ARMRINGR] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_ARMRINGR];
            HumItems[Grobal2.U_RINGL] = UseItems[Grobal2.U_RINGL] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_RINGL];
            HumItems[Grobal2.U_RINGR] = UseItems[Grobal2.U_RINGR] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_RINGR];
            HumItems[Grobal2.U_BUJUK] = UseItems[Grobal2.U_BUJUK] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_BUJUK];
            HumItems[Grobal2.U_BELT] = UseItems[Grobal2.U_BELT] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_BELT];
            HumItems[Grobal2.U_BOOTS] = UseItems[Grobal2.U_BOOTS] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_BOOTS];
            HumItems[Grobal2.U_CHARM] = UseItems[Grobal2.U_CHARM] == null ? HUtil32.DelfautItem : UseItems[Grobal2.U_CHARM];
            var BagItems = HumanRcd.Data.BagItems;
            if (BagItems == null)
            {
                BagItems = new UserItem[46];
            }
            for (var i = 0; i < ItemList.Count; i++)
            {
                if (i <= 46)
                {
                    BagItems[i] = ItemList[i];
                }
            }
            for (var i = 0; i < BagItems.Length; i++)
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
            for (var i = 0; i < MagicList.Count; i++)
            {
                if (i >= Grobal2.MaxMagicCount)
                {
                    break;
                }
                var UserMagic = MagicList[i];
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = new TMagicRcd();
                }
                HumMagic[i].wMagIdx = UserMagic.wMagIdx;
                HumMagic[i].btLevel = UserMagic.btLevel;
                HumMagic[i].btKey = UserMagic.btKey;
                HumMagic[i].nTranPoint = UserMagic.nTranPoint;
            }
            for (var i = 0; i < HumMagic.Length; i++)
            {
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = HUtil32.DetailtMagicRcd;
                }
            }
            var StorageItems = HumanRcd.Data.StorageItems;
            if (StorageItems == null)
            {
                StorageItems = new UserItem[50];
            }
            for (var i = 0; i < this.StorageItemList.Count; i++)
            {
                if (i >= StorageItems.Length)
                {
                    break;
                }
                StorageItems[i] = this.StorageItemList[i];
            }
            for (var i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = HUtil32.DelfautItem;
                }
            }
        }

        public void RefRankInfo(int nRankNo, string sRankName)
        {
            GuildRankNo = nRankNo;
            GuildRankName = sRankName;
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent >= Grobal2.CM_HIT || MsgList[i].wIdent <= Grobal2.CM_FIREHIT)
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.CM_SPELL)
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.CM_RUN)
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.CM_WALK)
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.CM_TURN)
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
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.CM_SITDOWN)
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
            if (M2Share.Config.CloseSpeedHackCheck)
            {
                return true;
            }
            int dwCheckTime;
            if (!M2Share.Config.DisableStruck) // 检查人物弯腰停留时间
            {
                dwCheckTime = HUtil32.GetTickCount() - StruckTick;
                if (M2Share.Config.StruckTime > dwCheckTime)
                {
                    dwDelayTime = M2Share.Config.StruckTime - dwCheckTime;
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
            if (!M2Share.Config.boControlActionInterval)
            {
                return true;
            }
            var dwActionIntervalTime = m_dwActionIntervalTime;
            switch (wIdent)
            {
                case Grobal2.CM_LONGHIT:
                    if (M2Share.Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_HIT:
                    if (M2Share.Config.boControlWalkHit && m_wOldIdent == Grobal2.CM_WALK && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime; // 走位攻击
                    }
                    if (M2Share.Config.boControlRunHit && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    break;
                case Grobal2.CM_RUN:
                    if (M2Share.Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_LONGHIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    if (M2Share.Config.boControlRunHit && m_wOldIdent == Grobal2.CM_HIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    if (M2Share.Config.boControlRunMagic && m_wOldIdent == Grobal2.CM_SPELL && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
                case Grobal2.CM_WALK:
                    if (M2Share.Config.boControlWalkHit && m_wOldIdent == Grobal2.CM_HIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime;// 走位攻击
                    }
                    if (M2Share.Config.boControlRunLongHit && m_wOldIdent == Grobal2.CM_LONGHIT && m_btOldDir != Direction)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_SPELL:
                    if (M2Share.Config.boControlRunMagic && m_wOldIdent == Grobal2.CM_RUN && m_btOldDir != Direction)
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

        private bool CheckItemsNeed(StdItem stdItem)
        {
            var result = true;
            var castle = M2Share.CastleMgr.IsCastleMember(this);
            switch (stdItem.Need)
            {
                case 6:
                    if (MyGuild == null)
                    {
                        result = false;
                    }
                    break;
                case 60:
                    if (MyGuild == null || GuildRankNo != 1)
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
                    if (castle == null || GuildRankNo != 1)
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
                    if (m_nMemberType != HUtil32.LoWord(stdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
                case 82:
                    if (m_nMemberType < HUtil32.LoWord(stdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
            }
            return result;
        }

        private void CheckMarry()
        {
            string sSayMsg;
            var boIsfound = false;
            var sUnMarryFileName = M2Share.BasePath + M2Share.Config.EnvirDir + "UnMarry.txt";
            if (File.Exists(sUnMarryFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(sUnMarryFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    if (string.Compare(LoadList[i], this.CharName, StringComparison.OrdinalIgnoreCase) == 0)
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
                    sSayMsg = string.Format(M2Share.g_sfUnMarryWoManLoginMsg, CharName, CharName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                m_sDearName = "";
                RefShowName();
            }
            m_DearHuman = M2Share.WorldEngine.GetPlayObject(m_sDearName);
            if (m_DearHuman != null)
            {
                m_DearHuman.m_DearHuman = this;
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineSelfMsg, m_sDearName, CharName, m_DearHuman.Envir.MapDesc, m_DearHuman.CurrX, m_DearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineDearMsg, m_sDearName, CharName, Envir.MapDesc, CurrX, CurrY);
                    m_DearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineSelfMsg, m_sDearName, CharName, m_DearHuman.Envir.MapDesc, m_DearHuman.CurrX, m_DearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineDearMsg, m_sDearName, CharName, Envir.MapDesc, CurrX, CurrY);
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
            var boIsfound = false;
            string sSayMsg;
            for (var i = 0; i < M2Share.g_UnForceMasterList.Count; i++) // 处理强行脱离师徒关系
            {
                if (string.Compare(M2Share.g_UnForceMasterList[i], this.CharName, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (Abil.Level >= M2Share.Config.MasterOKLevel)
                {
                    var Human = M2Share.WorldEngine.GetPlayObject(m_sMasterName);
                    if (Human != null && !Human.Death && !Human.Ghost)
                    {
                        sSayMsg = string.Format(M2Share.g_sYourMasterListUnMasterOKMsg, CharName);
                        Human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        if (CharName == Human.m_sMasterName)// 如果大徒弟则将师父上的名字去掉
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
                        if (Human.m_btCreditPoint + M2Share.Config.MasterOKCreditPoint <= byte.MaxValue)
                        {
                            Human.m_btCreditPoint += (byte)M2Share.Config.MasterOKCreditPoint;
                        }
                        Human.BonusPoint += M2Share.Config.nMasterOKBonusPoint;
                        Human.SendMsg(Human, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        // 如果师父不在线则保存到记录表中
                        boIsfound = false;
                        for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
                        {
                            if (string.Compare(M2Share.g_UnMasterList[i], this.CharName, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (string.Compare(M2Share.g_UnMasterList[i], this.CharName, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (m_btCreditPoint + M2Share.Config.MasterOKCreditPoint <= byte.MaxValue)
                {
                    m_btCreditPoint += (byte)M2Share.Config.MasterOKCreditPoint;
                }
                BonusPoint += M2Share.Config.nMasterOKBonusPoint;
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            }
            if (string.IsNullOrEmpty(m_sMasterName))
            {
                return;
            }
            if (m_boMaster) // 师父上线通知
            {
                m_MasterHuman = M2Share.WorldEngine.GetPlayObject(m_sMasterName);
                if (m_MasterHuman != null)
                {
                    m_MasterHuman.m_MasterHuman = this;
                    m_MasterList.Add(m_MasterHuman);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineSelfMsg, m_sMasterName, CharName, m_MasterHuman.Envir.MapDesc, m_MasterHuman.CurrX, m_MasterHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineMasterListMsg, m_sMasterName, CharName, Envir.MapDesc, CurrX, CurrY);
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
                    m_MasterHuman = M2Share.WorldEngine.GetPlayObject(m_sMasterName);
                    if (m_MasterHuman != null)
                    {
                        if (m_MasterHuman.m_sMasterName == CharName)
                        {
                            m_MasterHuman.m_MasterHuman = this;
                        }
                        m_MasterHuman.m_MasterList.Add(this);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineSelfMsg, m_sMasterName, CharName, m_MasterHuman.Envir.MapDesc, m_MasterHuman.CurrX, m_MasterHuman.CurrY);
                        SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineMasterMsg, m_sMasterName, CharName, Envir.MapDesc, CurrX, CurrY);
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
            sMyInfo = sMyInfo.Replace("%name", CharName);
            sMyInfo = sMyInfo.Replace("%map", Envir.MapDesc);
            sMyInfo = sMyInfo.Replace("%x", CurrX.ToString());
            sMyInfo = sMyInfo.Replace("%y", CurrY.ToString());
            sMyInfo = sMyInfo.Replace("%level", Abil.Level.ToString());
            sMyInfo = sMyInfo.Replace("%gold", Gold.ToString());
            sMyInfo = sMyInfo.Replace("%pk", PkPoint.ToString());
            sMyInfo = sMyInfo.Replace("%minhp", Abil.HP.ToString());
            sMyInfo = sMyInfo.Replace("%maxhp", Abil.MaxHP.ToString());
            sMyInfo = sMyInfo.Replace("%minmp", Abil.MP.ToString());
            sMyInfo = sMyInfo.Replace("%maxmp", Abil.MaxMP.ToString());
            sMyInfo = sMyInfo.Replace("%mindc", HUtil32.LoWord(Abil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%maxdc", HUtil32.HiWord(Abil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%minmc", HUtil32.LoWord(Abil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%maxmc", HUtil32.HiWord(Abil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%minsc", HUtil32.LoWord(Abil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%maxsc", HUtil32.HiWord(Abil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%logontime", m_dLogonTime.ToString());
            sMyInfo = sMyInfo.Replace("%logonint", ((HUtil32.GetTickCount() - m_dwLogonTick) / 60000).ToString());
            return sMyInfo;
        }

        private bool CheckItemBindUse(UserItem UserItem)
        {
            TItemBind ItemBind;
            var result = true;
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
                    if (string.Compare(ItemBind.sBindName, CharName, StringComparison.OrdinalIgnoreCase) == 0)
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

        private void ProcessClientPassword(ProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wParam == 0)
            {
                ProcessUserLineMsg("@" + M2Share.GameCommand.Unlock.CommandName);
                return;
            }
            var sData = ProcessMsg.Msg;
            var nLen = sData.Length;
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
                        if (M2Share.Config.LockDealAction)
                        {
                            m_boCanDeal = true;
                        }
                        if (M2Share.Config.LockDropAction)
                        {
                            m_boCanDrop = true;
                        }
                        if (M2Share.Config.LockWalkAction)
                        {
                            m_boCanWalk = true;
                        }
                        if (M2Share.Config.LockRunAction)
                        {
                            m_boCanRun = true;
                        }
                        if (M2Share.Config.LockHitAction)
                        {
                            m_boCanHit = true;
                        }
                        if (M2Share.Config.LockSpellAction)
                        {
                            m_boCanSpell = true;
                        }
                        if (M2Share.Config.LockSendMsgAction)
                        {
                            m_boCanSendMsg = true;
                        }
                        if (M2Share.Config.LockUserItemAction)
                        {
                            m_boCanUseItem = true;
                        }
                        if (M2Share.Config.LockInObModeAction)
                        {
                            ObMode = false;
                            AdminMode = false;
                        }
                        m_boLockLogoned = true;
                        SysMsg(M2Share.g_sPasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    if (m_boUnLockStoragePwd)
                    {
                        if (M2Share.Config.LockGetBackItemAction)
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
            var PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (GetFrontPosition(ref nX, ref nY))
                {
                    if (GetRecallXy(nX, nY, 3, ref n18, ref n1C))
                    {
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(MapName, n18, n1C, 0);
                    }
                }
                else
                {
                    SysMsg("召唤失败!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                SysMsg(Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }

        public void ReQuestGuildWar(string sGuildName)
        {
            if (!IsGuildMaster())
            {
                SysMsg("只有行会掌门人才能申请!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.ServerIndex != 0)
            {
                SysMsg("这个命令不能在本服务器上使用!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (Guild == null)
            {
                SysMsg("行会不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var boReQuestOK = false;
            var WarGuild = MyGuild.AddWarGuild(Guild);
            if (WarGuild != null)
            {
                if (Guild.AddWarGuild(MyGuild) == null)
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
                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, Guild.sGuildName);
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
            else if (M2Share.GetDenyChrNameList(CharName))
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
        /// <param name="serveraddr"></param>
        /// <param name="gamePort"></param>
        public void ChangeSnapsServer(string serveraddr, int gamePort)
        {
            this.SendMsg(this, Grobal2.RM_RECONNECTION, 0, 0, 0, 0, serveraddr + '/' + gamePort);
        }

        private void ProcessQueryValue(int Npc, string sData)
        {
            const string sIsInQVFilterListMsg = "你输入的文本中包含了非法字符，请重新出入";
            NormNpc NormNpc;
            var sRefMsg = string.Empty;
            if (!Ghost && !string.IsNullOrEmpty(m_sGotoNpcLabel))
            {
                sRefMsg = EDCode.DeCodeString(sData);
                //if (IsInGuildRankNameFilterList(sRefMsg))
                //{
                //    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, sIsInQVFilterListMsg);
                //    return;
                //}
            }
            switch (m_btValType)
            {
                case 0:
                    m_nSval[m_btValLabel] = sRefMsg;
                    break;
                case 1:
                    m_nMval[m_btValLabel] = HUtil32.Str_ToInt(sRefMsg, 0);
                    break;
            }
            switch (m_btValNPCType)
            {
                case 0:
                    NormNpc = (NormNpc)M2Share.WorldEngine.FindMerchant(Npc);
                    if (NormNpc == null)
                    {
                        NormNpc = (NormNpc)M2Share.WorldEngine.FindNpc(Npc);
                    }
                    if (NormNpc != null)
                    {
                        if (NormNpc.Envir == Envir && Math.Abs(NormNpc.CurrX - CurrX) <= 15 && Math.Abs(NormNpc.CurrY - CurrY) <= 15)
                        {
                            NormNpc.GotoLable(this, m_sGotoNpcLabel, false);
                        }
                    }
                    break;
                case 1:
                    if (M2Share.g_FunctionNPC != null)
                    {
                        M2Share.g_FunctionNPC.GotoLable(this, m_sGotoNpcLabel, false);
                    }
                    break;
                case 2:
                    if (M2Share.g_ManageNPC != null)
                    {
                        M2Share.g_ManageNPC.GotoLable(this, m_sGotoNpcLabel, false);
                    }
                    break;
            }
            m_sGotoNpcLabel = string.Empty;
        }

        private void ClientMerchantItemDlgSelect(int nParam1, int nParam2, int nParam3)
        {
            DlgItemIndex = 0;
            if (!Death && !Ghost)
            {
                NormNpc Npc = null;
                if (nParam1 == 0)
                {
                    return;
                }
                Npc = (NormNpc)M2Share.WorldEngine.FindMerchant(nParam1);
                if (Npc == null)
                {
                    Npc = (NormNpc)M2Share.WorldEngine.FindNpc(nParam1);
                }
                if (Npc != null)
                {
                    //LastNPC = Npc;
                }
                if (Npc == null)
                {
                    return;
                }
                if (Npc.Envir == Envir && Math.Abs(Npc.CurrX - CurrX) < 15 && Math.Abs(Npc.CurrY - CurrY) < 15 || Npc.m_boIsHide)
                {
                    DlgItemIndex = HUtil32.MakeLong(nParam2, nParam3);
                    var nTemp = 0;
                    if (TakeDlgItem && DlgItemIndex > 0)
                    {
                        nTemp = 255;
                        for (var i = 0; i < ItemList.Count; i++)
                        {
                            var UserItem = ItemList[i];
                            if (UserItem.wIndex == DlgItemIndex)
                            {
                                var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.wIndex);
                                if (StdItem != null)
                                {
                                    if (StdItem.NeedIdentify == 1)
                                    {
                                        // M2Share.AddGameDataLog('10' + #9 + m_sMapName + #9 +inttostr(m_nCurrX) + #9 + inttostr(m_nCurrY) + #9 +m_sCharName + #9 + StdItem.Name + #9 +inttostr(UserItem.MakeIndex) + #9 + '1' + #9 + m_sCharName);
                                        SendDelItems(UserItem);
                                        UserItem = null;
                                        ItemList.RemoveAt(i);
                                        DlgItemIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        nTemp = 0;
                    }
                    SendDefMessage(Grobal2.SM_ITEMDLGSELECT, 1, nTemp, 0, 0, "");
                    //Npc.m_OprCount = 0;
                    Npc.GotoLable(this, m_sGotoNpcLabel, false);
                    m_sGotoNpcLabel = string.Empty;
                }
            }
        }
    }
}