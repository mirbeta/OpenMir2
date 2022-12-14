using GameSvr.Actor;
using GameSvr.Event.Events;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Npc;
using GameSvr.Services;
using GameSvr.World;
using System.Text.RegularExpressions;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;

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
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.Item, mapItem) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(Grobal2.RM_ITEMHIDE, 0, mapItem.ActorId, CurrX, CurrY, "");
                        if (M2Share.GameLogGold)
                        {
                            M2Share.EventSource.AddEventLog(4, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.sSTRING_GOLDNAME
                                                               + "\t" + mapItem.Count + "\t" + '1' + "\t" + '0');
                        }
                        GoldChanged();
                        Dispose(mapItem);
                    }
                    else
                    {
                        Envir.AddToMap(CurrX, CurrY, CellType.Item, mapItem);
                    }
                }
                return result;
            }
            if (IsEnoughBag())
            {
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.Item, mapItem) == 1)
                {
                    var userItem = mapItem.UserItem;
                    var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem != null && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(userItem.Index)))
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, mapItem.ActorId, CurrX, CurrY, "");
                        AddItemToBag(userItem);
                        if (!M2Share.IsCheapStuff(stdItem.StdMode))
                        {
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(4, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name
                                                                   + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        Dispose(mapItem);
                        if (Race == ActorRace.Play)
                        {
                            this.SendAddItem(userItem);
                        }
                        result = true;
                    }
                    else
                    {
                        Dispose(userItem);
                        Envir.AddToMap(CurrX, CurrY, CellType.Item, mapItem);
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
                dwExp = MNKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(MNKillMonExpRate / 100 * dwExp);// 人物指定的杀怪经验倍数
                if (Envir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(Envir.Flag.nEXPRATE / 100 * dwExp);// 地图上指定杀怪经验倍数
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
                M2Share.EventSource.AddEventLog(12, MapName + "\t" + Abil.Level + "\t" + Abil.Exp + "\t" + ChrName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
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
            return WAbil.Weight + nWeight <= WAbil.MaxWeight;
        }

        public void SendAddItem(UserItem userItem)
        {
            var item = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (item == null)
            {
                return;
            }
            var clientItem = new ClientItem();
            item.GetUpgradeStdItem(userItem, ref clientItem);
            clientItem.Item.Name = CustomItem.GetItemName(userItem);
            clientItem.MakeIndex = userItem.MakeIndex;
            clientItem.Dura = userItem.Dura;
            clientItem.DuraMax = userItem.DuraMax;
            if (item.StdMode == 50)
            {
                clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
            }
            if (M2Share.StdModeMap.Contains(item.StdMode))
            {
                if (userItem.Desc[8] == 0)
                {
                    clientItem.Item.Shape = 0;
                }
                else
                {
                    clientItem.Item.Shape = 130;
                }
            }
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDITEM, ActorId, 0, 0, 1);
            SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
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

        private void SendSocket(string sMsg)
        {
            if (OffLineFlag)
            {
                return;
            }
            var msgHdr = new GameServerPacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = MNSocket,
                SessionId = MNGSocketIdx,
                Ident = Grobal2.GM_DATA
            };
            if (!string.IsNullOrEmpty(sMsg))
            {
                var bMsg = HUtil32.GetBytes(sMsg);
                msgHdr.PackLength = -bMsg.Length;
                var nSendBytes = Math.Abs(msgHdr.PackLength) + 20;
                using var memoryStream = new MemoryStream();
                using var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(nSendBytes);
                backingStream.Write(msgHdr.GetBuffer());
                if (bMsg.Length > 0)
                {
                    backingStream.Write(bMsg);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var data = new byte[memoryStream.Length];
                memoryStream.Read(data, 0, data.Length);
                M2Share.GateMgr.AddGateBuffer(MNGateIdx, data);
            }
        }

        private void SendSocket(ClientMesaagePacket defMsg)
        {
            SendSocket(defMsg, "");
        }

        internal virtual void SendSocket(ClientMesaagePacket defMsg, string sMsg)
        {
            if (OffLineFlag && defMsg.Ident != Grobal2.SM_OUTOFCONNECTION)
            {
                return;
            }
            var messageHead = new GameServerPacket
            {
                PacketCode = Grobal2.RUNGATECODE,
                Socket = MNSocket,
                SessionId = MNGSocketIdx,
                Ident = Grobal2.GM_DATA
            };
            using var memoryStream = new MemoryStream();
            using var backingStream = new BinaryWriter(memoryStream);
            byte[] bMsg = null;
            int nSendBytes;
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
                nSendBytes = messageHead.PackLength + GameServerPacket.PacketSize;
                backingStream.Write(nSendBytes);
                backingStream.Write(messageHead.GetBuffer());
                backingStream.Write(defMsg.GetBuffer());
            }
            else if (!string.IsNullOrEmpty(sMsg))
            {
                bMsg = HUtil32.GetBytes(sMsg);
                messageHead.PackLength = -bMsg.Length;
                nSendBytes = Math.Abs(messageHead.PackLength) + GameServerPacket.PacketSize;
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
            M2Share.GateMgr.AddGateBuffer(MNGateIdx, data);
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            MDefMsg = Grobal2.MakeDefaultMsg(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(MDefMsg, EDCode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(MDefMsg);
            }
        }

        private byte DayBright()
        {
            byte result;
            if (Envir.Flag.boDayLight)
            {
                return 0;
            }
            if (Envir.Flag.boDarkness)
            {
                return 1;
            }
            switch (MBtBright)
            {
                case 1:
                    result = 0;
                    break;
                case 3:
                    result = 1;
                    break;
                default:
                    result = 2;
                    break;
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
                var useItem = UseItems[i];
                if (useItem == null)
                {
                    continue;
                }
                if (useItem.Index <= 0)
                {
                    continue;
                }
                var stdItem = M2Share.WorldEngine.GetStdItem(useItem.Index);
                if (stdItem != null)
                {
                    if (stdItem.Shape == 126 || stdItem.Shape == 127 || stdItem.Shape == 128 || stdItem.Shape == 129)
                    {
                        SendDelItems(useItem);
                        useItem.Index = 0;
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
            if (PayMent == 2 || M2Share.Config.TestServer)
            {
                n08 = (HUtil32.GetTickCount() - MDwLogonTick) / 1000;
            }
            else
            {
                n08 = 0;
            }
            var sC = LoginIpAddr + "\t" + UserAccount + "\t" + ChrName + "\t" + n08 + "\t" + MDLogonTime.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + MNPayMode;
            M2Share.AddLogonCostLog(sC);
            if (MNPayMode == 2)
            {
                IdSrvClient.Instance.SendLogonCostMsg(UserAccount, n08 / 60);
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
                var canWalk = M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true) > 0)
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true) > 0)
                        {
                            CurrX += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 &&
                            Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) &&
                            Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true) > 0)
                        {
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true) > 0)
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true) > 0)
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (CurrX > 1 && CurrY > 1 &&
                            Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true) > 0)
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
                var canWalk = M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (CurrY > 2 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 3, true) > 0)
                        {
                            CurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (CurrX < Envir.Width - 3 && CurrY > 2 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY - 3, true) > 0)
                        {
                            CurrX += 3;
                            CurrY -= 3;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (CurrX < Envir.Width - 3 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY, true) > 0)
                        {
                            CurrX += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (CurrX < Envir.Width - 3 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY + 3, true) > 0)
                        {
                            CurrX += 3;
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 3, true) > 0)
                        {
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (CurrX > 2 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY + 3, true) > 0)
                        {
                            CurrX -= 3;
                            CurrY += 3;
                        }
                        break;
                    case Grobal2.DR_LEFT:
                        if (CurrX > 2 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY, true) > 0)
                        {
                            CurrX -= 3;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (CurrX > 2 && CurrY > 2 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY - 3, true) > 0)
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
            TwinHitSkill = boSwitch;
            if (TwinHitSkill)
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
            if (HUtil32.GetTickCount() - LatestFireHitTick > 10 * 1000)
            {
                LatestFireHitTick = HUtil32.GetTickCount();
                FireHitSkill = true;
                SysMsg(M2Share.sFireSpiritsSummoned, MsgColor.Green, MsgType.Hint);
                return true;
            }
            SysMsg(M2Share.sFireSpiritsFail, MsgColor.Red, MsgType.Hint);
            return false;
        }

        private bool AllowTwinHitSkill()
        {
            LatestTwinHitTick = HUtil32.GetTickCount();
            TwinHitSkill = true;
            SysMsg("twin hit skill charged", MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void ClientClickNpc(int actorId)
        {
            if (!MBoCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            if (Death || Ghost)
            {
                return;
            }
            if (HUtil32.GetTickCount() - MDwClickNpcTime > M2Share.Config.ClickNpcTime)
            {
                MDwClickNpcTime = HUtil32.GetTickCount();
                var normNpc = WorldServer.FindMerchant<Merchant>(actorId) ?? WorldServer.FindNpc<NormNpc>(actorId);
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
            if (PvpLevel() >= 2)
            {
                HomeMap = M2Share.Config.RedHomeMap;
                HomeX = M2Share.Config.RedHomeX;
                HomeY = M2Share.Config.RedHomeY;
                return;
            }
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        HomeMap = M2Share.StartPointList[i].m_sMapName;
                        HomeX = M2Share.StartPointList[i].m_nCurrX;
                        HomeY = M2Share.StartPointList[i].m_nCurrY;
                        break;
                    }
                }
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
                DealCreat.DealCancel();
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
            PlayObject playObject;
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
                        playObject = GroupOwner.GroupMembers[i];
                        if (!playObject.Death && Envir == playObject.Envir && Math.Abs(CurrX - playObject.CurrX) <= 12 && Math.Abs(CurrX - playObject.CurrX) <= 12)
                        {
                            sumlv = sumlv + playObject.Abil.Level;
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
                            playObject = GroupOwner.GroupMembers[i];
                            if (!playObject.Death && Envir == playObject.Envir && Math.Abs(CurrX - playObject.CurrX) <= 12 && Math.Abs(CurrX - playObject.CurrX) <= 12)
                            {
                                if (M2Share.Config.HighLevelKillMonFixExp)
                                {
                                    playObject.WinExp(HUtil32.Round(dwExp / n)); // 在高等级经验不变时，把组队的经验平均分配
                                }
                                else
                                {
                                    playObject.WinExp(HUtil32.Round(dwExp / sumlv * playObject.Abil.Level));
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
            if (MBtBright != M2Share.g_nGameTime)
            {
                MBtBright = (byte)M2Share.g_nGameTime;
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

        public override string GetBaseObjectInfo()
        {
            return this.ChrName + " 标识:" + this.ActorId + " 权限等级: " + this.Permission + " 管理模式: " + HUtil32.BoolToStr(this.AdminMode)
                + " 隐身模式: " + HUtil32.BoolToStr(this.ObMode) + " 无敌模式: " + HUtil32.BoolToStr(this.SuperMan) + " 地图:" + this.MapName + '(' + this.Envir.MapDesc + ')'
                + " 座标:" + this.CurrX + ':' + this.CurrY + " 等级:" + this.Abil.Level + " 转生等级:" + MBtReLevel
                + " 经验:" + this.Abil.Exp + " 生命值: " + this.WAbil.HP + '-' + this.WAbil.MaxHP + " 魔法值: " + this.WAbil.MP + '-' + this.WAbil.MaxMP
                + " 攻击力: " + HUtil32.LoWord(this.WAbil.DC) + '-' + HUtil32.HiWord(this.WAbil.DC) + " 魔法力: " + HUtil32.LoWord(this.WAbil.MC) + '-'
                + HUtil32.HiWord(this.WAbil.MC) + " 道术: " + HUtil32.LoWord(this.WAbil.SC) + '-' + HUtil32.HiWord(this.WAbil.SC)
                + " 防御力: " + HUtil32.LoWord(this.WAbil.AC) + '-' + HUtil32.HiWord(this.WAbil.AC) + " 魔防力: " + HUtil32.LoWord(this.WAbil.MAC)
                + '-' + HUtil32.HiWord(this.WAbil.MAC) + " 准确:" + this.HitPoint + " 敏捷:" + this.SpeedPoint + " 速度:" + this.HitSpeed
                + " 仓库密码:" + MSStoragePwd + " 登录IP:" + LoginIpAddr + '(' + LoginIpLocal + ')' + " 登录帐号:" + UserAccount + " 登录时间:" + MDLogonTime
                + " 在线时长(分钟):" + (HUtil32.GetTickCount() - MDwLogonTick) / 60000 + " 登录模式:" + PayMent + ' ' + M2Share.Config.GameGoldName + ':' + MNGameGold
                + ' ' + M2Share.Config.GamePointName + ':' + MNGamePoint + ' ' + M2Share.Config.PayMentPointName + ':' + MNPayMentPoint + " 会员类型:" + MNMemberType
                + " 会员等级:" + MNMemberLevel + " 经验倍数:" + MNKillMonExpRate / 100 + " 攻击倍数:" + MNPowerRate / 100 + " 声望值:" + MBtCreditPoint;
        }

        private int GetDigUpMsgCount()
        {
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < MsgList.Count; i++)
                {
                    var sendMessage = MsgList[i];
                    if (sendMessage.wIdent == Grobal2.CM_BUTCH)
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
            int s10;
            string s14;
            if (nGold > 0)
            {
                s10 = 14;
                s14 = "增加完成";
            }
            else
            {
                s10 = 13;
                s14 = "以删减";
            }
            SysMsg(sChrName + " 的金币 " + nGold + " 金币" + s14, MsgColor.Green, MsgType.Hint);
            if (M2Share.GameLogGold)
            {
                M2Share.EventSource.AddEventLog(s10, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + sChrName);
            }
        }

        public void ClearStatusTime()
        {
            this.StatusArr = new ushort[15];
        }

        private void SendMapDescription()
        {
            var nMusicid = -1;
            if (Envir.Flag.boMUSIC)
            {
                nMusicid = Envir.Flag.nMUSICID;
            }
            SendDefMessage(Grobal2.SM_MAPDESCRIPTION, nMusicid, 0, 0, 0, Envir.MapDesc);
        }

        private void SendWhisperMsg(PlayObject playObject)
        {
            if (playObject == this)
            {
                return;
            }
            if (playObject.Permission >= 9 || Permission >= 9)
            {
                return;
            }
            if (M2Share.WorldEngine.PlayObjectCount < M2Share.Config.nSendWhisperPlayCount + M2Share.RandomNumber.Random(5))
            {
                return;
            }
        }

        public void ChangeSpaceMove(Envirnoment envir, short nX, short nY)
        {
            MSSwitchMapName = envir.MapName;
            MNSwitchMapX = nX;
            MNSwitchMapY = nY;
            MBoSwitchData = true;
            MNServerIndex = envir.ServerIndex;
            MBoEmergencyClose = true;
            MBoReconnection = true;
        }

        private void ReadAllBook()
        {
            for (var i = 0; i < M2Share.WorldEngine.MagicList.Count; i++)
            {
                var magic = M2Share.WorldEngine.MagicList[i];
                var userMagic = new UserMagic
                {
                    Magic = magic,
                    MagIdx = magic.MagicId,
                    Level = 2,
                    Key = (char)0
                };
                userMagic.Level = 0;
                userMagic.TranPoint = 100000;
                MagicList.Add(userMagic);
                SendAddMagic(userMagic);
            }
        }

        private void SendGoldInfo(bool boSendName)
        {
            var sMsg = string.Empty;
            if (MNSoftVersionDateEx == 0)
            {
                return;
            }
            if (boSendName)
            {
                sMsg = M2Share.Config.GameGoldName + '\r' + M2Share.Config.GamePointName;
            }
            SendDefMessage(Grobal2.SM_GAMEGOLDNAME, MNGameGold, HUtil32.LoWord(MNGamePoint), HUtil32.HiWord(MNGamePoint), 0, sMsg);
        }

        private void SendServerConfig()
        {
            if (MNSoftVersionDateEx == 0)
            {
                return;
            }

            if (M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll)
            {
            }
            else
            {
                if (M2Share.Config.boRunHuman || Envir.Flag.boRUNHUMAN)
                {
                }
                if (M2Share.Config.boRunMon || Envir.Flag.boRUNMON)
                {
                }
                if (M2Share.Config.boRunNpc)
                {
                }
                if (M2Share.Config.boWarDisHumRun)
                {
                }
            }
            var sMsg = EDCode.EncodeBuffer(M2Share.Config.ClientConf);
            //var nRecog = HUtil32.MakeLong(HUtil32.MakeWord(nRunHuman, nRunMon), HUtil32.MakeWord(nRunNpc, nWarRunAll));
            //var nParam = (short)HUtil32.MakeWord(5, 0);
            SendDefMessage(Grobal2.SM_SERVERCONFIG, 0, 0, 0, 0, sMsg);
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
        protected bool CretInNearXy(BaseObject targeBaseObject, int nX, int nY)
        {
            if (Envir == null)
            {
                M2Share.Log.Error("CretInNearXY nil PEnvir");
                return false;
            }
            for (var cX = nX - 1; cX <= nX + 1; cX++)
            {
                for (var cY = nY - 1; cY <= nY + 1; cY++)
                {
                    var cellSuccess = false;
                    var cellInfo = Envir.GetCellInfo(cX, cY, ref cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            var cellObject = cellInfo.ObjList[i];
                            if (cellObject.ActorObject)
                            {
                                var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                if (baseObject != null)
                                {
                                    if (!baseObject.Ghost && baseObject == targeBaseObject)
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

        internal void SendUseItems()
        {
            var sSendMsg = string.Empty;
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    var item = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (item != null)
                    {
                        var clientItem = new ClientItem();
                        item.GetUpgradeStdItem(UseItems[i], ref clientItem);
                        //Item.GetItemAddValue(UseItems[i], ref ClientItem.Item);
                        clientItem.Item.Name = CustomItem.GetItemName(UseItems[i]);
                        clientItem.Dura = UseItems[i].Dura;
                        clientItem.DuraMax = UseItems[i].DuraMax;
                        clientItem.MakeIndex = UseItems[i].MakeIndex;
                        if (i == Grobal2.U_DRESS)
                        {
                            ChangeItemWithLevel(ref clientItem, Abil.Level);
                        }
                        ChangeItemByJob(ref clientItem, Abil.Level);
                        sSendMsg = sSendMsg + i + '/' + EDCode.EncodeBuffer(clientItem) + '/';
                    }
                }
            }
            if (sSendMsg != "")
            {
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSEITEMS, 0, 0, 0, 0);
                SendSocket(MDefMsg, sSendMsg);
            }
        }

        private void SendUseMagic()
        {
            var sSendMsg = string.Empty;
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                var clientMagic = new ClientMagic();
                clientMagic.Key = userMagic.Key;
                clientMagic.Level = userMagic.Level;
                clientMagic.CurTrain = userMagic.TranPoint;
                clientMagic.Def = userMagic.Magic;
                sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientMagic) + '/';
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDMYMAGIC, 0, 0, 0, (short)MagicList.Count);
                SendSocket(MDefMsg, sSendMsg);
            }
        }

        private bool UseStdmodeFunItem(StdItem stdItem)
        {
            var result = false;
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@StdModeFunc" + stdItem.AniCount, false);
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
            NakedAbility bonusTick = null;
            NakedAbility nakedAbil = null;
            switch (Job)
            {
                case PlayJob.Warrior:
                    bonusTick = M2Share.Config.BonusAbilofWarr;
                    nakedAbil = M2Share.Config.NakedAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    bonusTick = M2Share.Config.BonusAbilofWizard;
                    nakedAbil = M2Share.Config.NakedAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    bonusTick = M2Share.Config.BonusAbilofTaos;
                    nakedAbil = M2Share.Config.NakedAbilofTaos;
                    break;
            }
            var adc = (short)(BonusAbil.DC / bonusTick.DC);
            var amc = (short)(BonusAbil.MC / bonusTick.MC);
            var asc = (short)(BonusAbil.SC / bonusTick.SC);
            var aac = (short)(BonusAbil.AC / bonusTick.AC);
            var amac = (short)(BonusAbil.MAC / bonusTick.MAC);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.DC, adc, ref ldc, ref hdc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.MC, amc, ref lmc, ref hmc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.SC, asc, ref lsc, ref hsc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.AC, aac, ref lac, ref hac);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.MAC, amac, ref lmac, ref hmac);
            Abil.DC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(Abil.DC) + ldc), (ushort)(HUtil32.HiWord(Abil.DC) + hdc));
            Abil.MC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(Abil.MC) + lmc), (ushort)(HUtil32.HiWord(Abil.MC) + hmc));
            Abil.SC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(Abil.SC) + lsc), (ushort)(HUtil32.HiWord(Abil.SC) + hsc));
            Abil.AC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(Abil.AC) + lac), (ushort)(HUtil32.HiWord(Abil.AC) + hac));
            Abil.MAC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(Abil.MAC) + lmac), (ushort)(HUtil32.HiWord(Abil.MAC) + hmac));
            Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + BonusAbil.HP / bonusTick.HP);
            Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + BonusAbil.MP / bonusTick.MP);
        }

        private void ClientAdjustBonus(int nPoint, string sMsg)
        {
            var bonusAbil = new NakedAbility();
            var nTotleUsePoint = bonusAbil.DC + bonusAbil.MC + bonusAbil.SC + bonusAbil.AC + bonusAbil.MAC + bonusAbil.HP + bonusAbil.MP + bonusAbil.Hit + bonusAbil.Speed + bonusAbil.Reserved;
            if (nPoint + nTotleUsePoint == BonusPoint)
            {
                BonusPoint = nPoint;
                this.BonusAbil.DC += bonusAbil.DC;
                this.BonusAbil.MC += bonusAbil.MC;
                this.BonusAbil.SC += bonusAbil.SC;
                this.BonusAbil.AC += bonusAbil.AC;
                this.BonusAbil.MAC += bonusAbil.MAC;
                this.BonusAbil.HP += bonusAbil.HP;
                this.BonusAbil.MP += bonusAbil.MP;
                this.BonusAbil.Hit += bonusAbil.Hit;
                this.BonusAbil.Speed += bonusAbil.Speed;
                this.BonusAbil.Reserved += bonusAbil.Reserved;
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
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADJUST_BONUS, BonusPoint, 0, 0, 0);
            SendSocket(MDefMsg, sSendMsg);
        }

        public void PkDie(PlayObject playObject)
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
            if (playObject.Abil.Level - Abil.Level > M2Share.Config.HumanLevelDiffer)
            {
                if (!playObject.IsGoodKilling(this))
                {
                    playObject.IncPkPoint(M2Share.Config.KillHumanAddPKPoint);
                    playObject.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                    SysMsg(Format(M2Share.g_sYouKilledByMsg, LastHiter.ChrName), MsgColor.Red, MsgType.Hint);
                    playObject.AddBodyLuck(-M2Share.Config.KillHumanDecLuckPoint);
                    if (PvpLevel() < 1)
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            playObject.MakeWeaponUnlock();
                        }
                    }
                    if (M2Share.g_FunctionNPC != null)
                    {
                        M2Share.g_FunctionNPC.GotoLable(playObject, "@OnMurder", false);
                        M2Share.g_FunctionNPC.GotoLable(this, "@Murdered", false);
                    }
                }
                else
                {
                    playObject.SysMsg(M2Share.g_sYouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                }
                return;
            }
            if (boWinLEvel)
            {
                if (playObject.Abil.Level + nWinLevel <= M2Share.MAXUPLEVEL)
                {
                    playObject.Abil.Level += (byte)nWinLevel;
                }
                else
                {
                    playObject.Abil.Level = M2Share.MAXUPLEVEL;
                }
                playObject.HasLevelUp(playObject.Abil.Level - nWinLevel);
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
                playObject.WinExp(nWinExp);
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
            PlayObject playObject;
            var sSendMsg = "";
            for (var i = 0; i < GroupMembers.Count; i++)
            {
                playObject = GroupMembers[i];
                sSendMsg = sSendMsg + playObject.ChrName + '/';
            }
            for (var i = 0; i < GroupMembers.Count; i++)
            {
                playObject = GroupMembers[i];
                playObject.SendDefMessage(Grobal2.SM_GROUPMEMBERS, 0, 0, 0, 0, sSendMsg);
            }
        }

        protected ushort GetSpellPoint(UserMagic userMagic)
        {
            return (ushort)(HUtil32.Round(userMagic.Magic.Spell / (userMagic.Magic.TrainLv + 1) * (userMagic.Level + 1)) + userMagic.Magic.DefSpell);
        }

        private bool DoMotaeboCanMotaebo(BaseObject baseObject, int nMagicLevel)
        {
            if (Abil.Level > baseObject.Abil.Level && !baseObject.StickMode) //当前玩家等级大于目标等级，并且目标可以被冲撞
            {
                var nC = Abil.Level - baseObject.Abil.Level;
                if (M2Share.RandomNumber.Random(20) < nMagicLevel * 4 + 6 + nC)
                {
                    if (IsProperTarget(baseObject))
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
            BaseObject baseObject34 = null;
            short nX = 0;
            short nY = 0;
            var result = false;
            var bo35 = true;
            var n24 = nMagicLevel + 1;
            var n28 = n24;
            Direction = nDir;
            var poseCreate = GetPoseCreate();
            if (poseCreate != null)
            {
                for (var i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    poseCreate = GetPoseCreate();
                    if (poseCreate != null)
                    {
                        n28 = 0;
                        if (!DoMotaeboCanMotaebo(poseCreate, nMagicLevel))
                        {
                            break;
                        }
                        if (nMagicLevel >= 3)
                        {
                            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref nX, ref nY))
                            {
                                BaseObject baseObject30 = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                                if (baseObject30 != null && DoMotaeboCanMotaebo(baseObject30, nMagicLevel))
                                {
                                    baseObject30.CharPushed(Direction, 1);
                                }
                            }
                        }
                        baseObject34 = poseCreate;
                        if (poseCreate.CharPushed(Direction, 1) != 1)
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
            if (baseObject34 != null)
            {
                if (n24 < 0)
                {
                    n24 = 0;
                }
                nDmg = M2Share.RandomNumber.Random((n24 + 1) * 10) + (n24 + 1) * 10;
                nDmg = baseObject34.GetHitStruckDamage(this, nDmg);
                baseObject34.StruckDamage((ushort)nDmg);
                baseObject34.SendRefMsg(Grobal2.RM_STRUCK, nDmg, baseObject34.WAbil.HP, baseObject34.WAbil.MaxHP, ActorId, "");
                if (baseObject34.Race != ActorRace.Play)
                {
                    baseObject34.SendMsg(baseObject34, Grobal2.RM_STRUCK, nDmg, baseObject34.WAbil.HP, baseObject34.WAbil.MaxHP, ActorId, "");
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
                StruckDamage((ushort)nDmg);
                SendRefMsg(Grobal2.RM_STRUCK, nDmg, WAbil.HP, WAbil.MaxHP, 0, "");
            }
            return result;
        }

        private bool DoSpell(UserMagic userMagic, short targetX, short targetY, BaseObject baseObject)
        {
            var result = false;
            try
            {
                if (!M2Share.MagicMgr.IsWarrSkill(userMagic.MagIdx))
                {
                    var nSpellPoint = GetSpellPoint(userMagic);
                    if (nSpellPoint > 0)
                    {
                        if (WAbil.MP < nSpellPoint)
                        {
                            return result;
                        }
                        DamageSpell(nSpellPoint);
                        HealthSpellChanged();
                    }
                    result = M2Share.MagicMgr.DoSpell(this, userMagic, targetX, targetY, baseObject);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(Format("[Exception] TPlayObject.DoSpell MagID:{0} X:{1} Y:{2}", userMagic.MagIdx, targetX, targetY));
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
                        DoDamageWeapon((ushort)(M2Share.RandomNumber.Random(15) + 5));
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

        private void SendSaveItemList(int merchantId)
        {
            var sSendMsg = string.Empty;
            var maxCount = StorageItemList.Count;
            var page = (short)Math.Ceiling(maxCount / 50f);

            for (int p = 0; p < page; p++)
            {
                var startCount = p * 50;
                var endCount = startCount + 50;
                if (endCount > maxCount)
                {
                    endCount = maxCount;
                }
                for (var i = startCount; i < endCount; i++)
                {
                    var userItem = StorageItemList[i];
                    var item = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (item != null)
                    {
                        ClientItem clientItem = new ClientItem();
                        item.GetUpgradeStdItem(userItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Item.Name = CustomItem.GetItemName(userItem);
                        clientItem.Dura = userItem.Dura;
                        clientItem.DuraMax = userItem.DuraMax;
                        clientItem.MakeIndex = userItem.MakeIndex;
                        sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + '/';
                    }
                }
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SAVEITEMLIST, merchantId, 0, p, (page - 1) < 0 ? 0 : page - 1);
                SendSocket(MDefMsg, sSendMsg);
            }
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

        private void SendDelItemList(IList<DeleteItem> itemList)
        {
            var s10 = string.Empty;
            for (var i = 0; i < itemList.Count; i++)
            {
                s10 = s10 + itemList[i].ItemName + '/' + itemList[i].MakeIndex + '/';
            }
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELITEMS, 0, 0, 0, itemList.Count);
            SendSocket(MDefMsg, EDCode.EncodeString(s10));
        }

        public void SendDelItems(UserItem userItem)
        {
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                var clientItem = new ClientItem();
                stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                clientItem.Item.Name = CustomItem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELITEM, ActorId, 0, 0, 1);
                SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        public void SendUpdateItem(UserItem userItem)
        {
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                var clientItem = new ClientItem();
                stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                clientItem.Item.Name = CustomItem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private void SendUpdateItemWithLevel(UserItem userItem, byte level)
        {
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                var clientItem = new ClientItem();
                stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                clientItem.Item.Name = CustomItem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ChangeItemWithLevel(ref clientItem, level);
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private void SendUpdateItemByJob(UserItem userItem, byte level)
        {
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                var clientItem = new ClientItem();
                stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                clientItem.Item.Name = CustomItem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ChangeItemByJob(ref clientItem, level);
                MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private bool CheckTakeOnItems(int nWhere, ref ClientItem clientItem)
        {
            var result = false;
            switch (clientItem.Item.StdMode)
            {
                case 10 when Gender != PlayGender.Man:
                    SysMsg(M2Share.sWearNotOfWoMan, MsgColor.Red, MsgType.Hint);
                    return false;
                case 11 when Gender != PlayGender.WoMan:
                    SysMsg(M2Share.sWearNotOfMan, MsgColor.Red, MsgType.Hint);
                    return false;
            }
            if (nWhere == 1 || nWhere == 2)
            {
                if (clientItem.Item.Weight > WAbil.MaxHandWeight)
                {
                    SysMsg(M2Share.sHandWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            else
            {
                if (clientItem.Item.Weight + GetUserItemWeitht(nWhere) > WAbil.MaxWearWeight)
                {
                    SysMsg(M2Share.sWearWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            switch (clientItem.Item.Need)
            {
                case 0:
                    if (Abil.Level >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 1:
                    if (HUtil32.HiByte(WAbil.DC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 10:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && Abil.Level >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 11:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.DC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 12:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.MC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 13:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.SC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 2:
                    if (HUtil32.HiByte(WAbil.MC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 3:
                    if (HUtil32.HiByte(WAbil.SC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 4:
                    if (MBtReLevel >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 40:
                    if (MBtReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (Abil.Level >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    if (MBtReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.DC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    if (MBtReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.MC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    if (MBtReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.SC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    if (MBtReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (MBtCreditPoint >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    if (MBtCreditPoint >= clientItem.Item.NeedLevel)
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
                        if (Abil.Level >= clientItem.Item.NeedLevel)
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
                    if (MNMemberType != 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 81:
                    if (MNMemberType == HUtil32.LoByte(clientItem.Item.NeedLevel) && MNMemberLevel >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberTypeNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 82:
                    if (MNMemberType >= HUtil32.LoByte(clientItem.Item.NeedLevel) && MNMemberLevel >= HUtil32.HiByte(clientItem.Item.NeedLevel))
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
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (stdItem != null)
                    {
                        n14 += stdItem.Weight;
                    }
                }
            }
            return n14;
        }

        private bool EatItems(StdItem stdItem, UserItem userItem)
        {
            var result = false;
            if (Envir.Flag.boNODRUG)
            {
                SysMsg(M2Share.sCanotUseDrugOnThisMap, MsgColor.Red, MsgType.Hint);
                return false;
            }
            switch (stdItem.StdMode)
            {
                case 0:
                    switch (stdItem.Shape)
                    {
                        case 1:
                            IncHealthSpell(stdItem.AC, stdItem.MAC);
                            result = true;
                            break;
                        case 2:
                            UserUnLockDurg = true;
                            result = true;
                            break;
                        case 3:
                            IncHealthSpell(HUtil32.Round(WAbil.MaxHP / 100 * stdItem.AC), HUtil32.Round(WAbil.MaxMP / 100 * stdItem.MAC));
                            result = true;
                            break;
                        default:
                            if (stdItem.AC > 0)
                            {
                                IncHealth += stdItem.AC;
                            }
                            if (stdItem.MAC > 0)
                            {
                                IncSpell += stdItem.MAC;
                            }
                            result = true;
                            break;
                    }
                    break;
                case 1:
                    var nOldStatus = GetMyStatus();
                    HungerStatus += stdItem.DuraMax / 10;
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
                    switch (stdItem.Shape)
                    {
                        case 12:
                            var boNeedRecalc = false;
                            if (HUtil32.LoByte(stdItem.DC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_DCUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_DCUP], HUtil32.LoByte(stdItem.DC));
                                ExtraAbilFlag[AbilConst.EABIL_DCUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_DCUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_DCUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("攻击力瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "秒。", MsgColor.Blue, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (HUtil32.LoByte(stdItem.MC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_MCUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_MCUP], HUtil32.LoByte(stdItem.MC));
                                ExtraAbilFlag[AbilConst.EABIL_MCUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_MCUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_MCUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("魔法力瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "sec.", MsgColor.Blue, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (HUtil32.LoByte(stdItem.SC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_SCUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_SCUP], HUtil32.LoByte(stdItem.SC));
                                ExtraAbilFlag[AbilConst.EABIL_SCUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_SCUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_SCUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("精神力瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "秒。", MsgColor.Blue, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (HUtil32.HiByte(stdItem.AC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_HITSPEEDUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_HITSPEEDUP], HUtil32.HiByte(stdItem.AC));
                                ExtraAbilFlag[AbilConst.EABIL_HITSPEEDUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_HITSPEEDUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_HITSPEEDUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("攻速瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "秒。", MsgColor.Blue, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (HUtil32.LoByte(stdItem.AC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_HPUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_HPUP], HUtil32.LoByte(stdItem.AC));
                                ExtraAbilFlag[AbilConst.EABIL_HPUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_HPUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_HPUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("体力值瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "秒。", MsgColor.Blue, MsgType.Hint);
                                boNeedRecalc = true;
                            }
                            if (HUtil32.LoByte(stdItem.MAC) > 0)
                            {
                                ExtraAbil[AbilConst.EABIL_MPUP] = (ushort)HUtil32._MAX(ExtraAbil[AbilConst.EABIL_MPUP], HUtil32.LoByte(stdItem.MAC));
                                ExtraAbilFlag[AbilConst.EABIL_MPUP] = 0;
                                ExtraAbilTimes[AbilConst.EABIL_MPUP] = HUtil32._MAX(ExtraAbilTimes[AbilConst.EABIL_MPUP], HUtil32.GetTickCount() + HUtil32.HiByte(stdItem.DC) * 60 * 1000 + HUtil32.HiByte(stdItem.MAC) * 1000);
                                SysMsg("魔法值瞬间提高" + (HUtil32.HiByte(stdItem.DC) + HUtil32.HiByte(stdItem.MAC) / 60) + "分" + (HUtil32.HiByte(stdItem.MAC) % 60) + "秒。", MsgColor.Blue, MsgType.Hint);
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
                            GetExp(stdItem.DuraMax);
                            result = true;
                            break;
                        default:
                            result = EatUseItems(stdItem.Shape);
                            break;
                    }
                    break;
            }
            return result;
        }

        private bool ReadBook(StdItem stdItem)
        {
            var result = false;
            var magic = M2Share.WorldEngine.FindMagic(stdItem.Name);
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.MagicId))
                {
                    if (magic.Job == 99 || magic.Job == (byte)Job)
                    {
                        if (Abil.Level >= magic.TrainLevel[0])
                        {
                            var userMagic = new UserMagic
                            {
                                Magic = magic,
                                MagIdx = magic.MagicId,
                                Key = (char)0,
                                Level = 0,
                                TranPoint = 0
                            };
                            MagicList.Add(userMagic);
                            RecalcAbilitys();
                            if (Race == ActorRace.Play)
                            {
                                this.SendAddMagic(userMagic);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public void SendAddMagic(UserMagic userMagic)
        {
            var clientMagic = new ClientMagic
            {
                Key = userMagic.Key,
                Level = userMagic.Level,
                CurTrain = userMagic.TranPoint,
                Def = userMagic.Magic
            };
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_ADDMAGIC, 0, 0, 0, 1);
            SendSocket(MDefMsg, EDCode.EncodeBuffer(clientMagic));
        }

        internal void SendDelMagic(UserMagic userMagic)
        {
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELMAGIC, userMagic.MagIdx, 0, 0, 1);
            SendSocket(MDefMsg);
        }

        /// <summary>
        /// 使用物品
        /// </summary>
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
                            var castle = M2Share.CastleMgr.IsCastleMember(this);
                            if (castle != null && castle.IsMasterGuild(MyGuild))
                            {
                                BaseObjectMove(castle.HomeMap, castle.GetHomeX(), castle.GetHomeY());
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
            if (envir != Envir && Race == ActorRace.Play)
            {
                MBoTimeRecall = false;
            }
        }

        private void ChangeServerMakeSlave(SlaveInfo slaveInfo)
        {
            int nSlavecount;
            if (Job == PlayJob.Taoist)
            {
                nSlavecount = 1;
            }
            else
            {
                nSlavecount = 5;
            }
            var baseObject = MakeSlave(slaveInfo.SlaveName, 3, slaveInfo.SlaveLevel, nSlavecount, slaveInfo.RoyaltySec);
            if (baseObject != null)
            {
                baseObject.KillMonCount = slaveInfo.KillCount;
                baseObject.SlaveExpLevel = slaveInfo.SlaveExpLevel;
                baseObject.WAbil.HP = slaveInfo.nHP;
                baseObject.WAbil.MP = slaveInfo.nMP;
                if (1500 - slaveInfo.SlaveLevel * 200 < baseObject.WalkSpeed)
                {
                    baseObject.WalkSpeed = (1500 - slaveInfo.SlaveLevel) * 200;
                }
                if (2000 - slaveInfo.SlaveLevel * 200 < baseObject.NextHitTime)
                {
                    baseObject.WalkSpeed = (2000 - slaveInfo.SlaveLevel) * 200;
                }
                RecalcAbilitys();
            }
        }

        private void SendDelDealItem(UserItem userItem)
        {
            if (DealCreat != null)
            {
                var pStdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (pStdItem != null)
                {
                    var clientItem = new ClientItem();
                    pStdItem.GetUpgradeStdItem(userItem, ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(userItem);
                    clientItem.MakeIndex = userItem.MakeIndex;
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEDELITEM, ActorId, 0, 0, 1);
                    DealCreat.SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
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

        private void SendAddDealItem(UserItem userItem)
        {
            SendDefMessage(Grobal2.SM_DEALADDITEM_OK, 0, 0, 0, 0, "");
            if (DealCreat != null)
            {
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    var clientItem = new ClientItem();
                    stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(userItem);
                    clientItem.MakeIndex = userItem.MakeIndex;
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_DEALREMOTEADDITEM, ActorId, 0, 0, 1);
                    DealCreat.SendSocket(MDefMsg, EDCode.EncodeBuffer(clientItem));
                    DealCreat.DealLastTick = HUtil32.GetTickCount();
                    DealLastTick = HUtil32.GetTickCount();
                }
            }
        }

        private void OpenDealDlg(BaseObject baseObject)
        {
            DealCreat = (PlayObject)baseObject;
            if (DealCreat == null)
            {
                return;
            }
            Dealing = true;
            GetBackDealItems();
            SendDefMessage(Grobal2.SM_DEALMENU, 0, 0, 0, 0, DealCreat.ChrName);
            DealLastTick = HUtil32.GetTickCount();
        }

        private void JoinGroup(PlayObject playObject)
        {
            GroupOwner = playObject;
            SendGroupText(Format(M2Share.g_sJoinGroup, ChrName));
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
            UserItem userItem = null;
            if (ItemList.Count >= Grobal2.MAXBAGITEM)
            {
                return;
            }
            var nRandom = M2Share.RandomNumber.Random(M2Share.Config.StoneTypeRate);
            if (nRandom >= M2Share.Config.GoldStoneMin && nRandom <= M2Share.Config.GoldStoneMax)
            {
                userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.GoldStone, ref userItem))
                {
                    userItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(userItem);
                    WeightChanged();
                    SendAddItem(userItem);
                }
                else
                {
                    Dispose(userItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.SilverStoneMin && nRandom <= M2Share.Config.SilverStoneMax)
            {
                userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.SilverStone, ref userItem))
                {
                    userItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(userItem);
                    WeightChanged();
                    SendAddItem(userItem);
                }
                else
                {
                    Dispose(userItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.SteelStoneMin && nRandom <= M2Share.Config.SteelStoneMax)
            {
                userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.SteelStone, ref userItem))
                {
                    userItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(userItem);
                    WeightChanged();
                    SendAddItem(userItem);
                }
                else
                {
                    Dispose(userItem);
                }
                return;
            }
            if (nRandom >= M2Share.Config.BlackStoneMin && nRandom <= M2Share.Config.BlackStoneMax)
            {
                userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.BlackStone, ref userItem))
                {
                    userItem.Dura = MakeMineRandomDrua();
                    ItemList.Add(userItem);
                    WeightChanged();
                    SendAddItem(userItem);
                }
                else
                {
                    Dispose(userItem);
                }
                return;
            }
            userItem = new UserItem();
            if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.CopperStone, ref userItem))
            {
                userItem.Dura = MakeMineRandomDrua();
                ItemList.Add(userItem);
                WeightChanged();
                SendAddItem(userItem);
            }
            else
            {
                Dispose(userItem);
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
            UserItem result = null;
            nParam = 0;
            nDura = 0;
            nCount = 0;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (userItem.Dura > nDura)
                    {
                        nDura = userItem.Dura;
                        result = userItem;
                    }
                    nParam += userItem.Dura;
                    if (result == null)
                    {
                        result = userItem;
                    }
                    nCount++;
                }
            }
            return result;
        }

        public bool QuestTakeCheckItem(UserItem checkItem)
        {
            var result = false;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem == checkItem)
                {
                    SendDelItems(userItem);
                    Dispose(userItem);
                    ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == checkItem)
                {
                    SendDelItems(UseItems[i]);
                    UseItems[i].Index = 0;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void RefRankInfo(short nRankNo, string sRankName)
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
                    MBtOldDir = Direction;
                    return false;
                }
            }
            // 检查二个不同操作之间所需间隔时间
            dwCheckTime = HUtil32.GetTickCount() - MDwActionTick;
            if (MBoTestSpeedMode)
            {
                SysMsg("间隔: " + dwCheckTime, MsgColor.Blue, MsgType.Notice);
            }
            if (MWOldIdent == wIdent)
            {
                // 当二次操作一样时，则将 boFirst 设置为 真 ，退出由调用函数本身检查二个相同操作之间的间隔时间
                return true;
            }
            if (!M2Share.Config.boControlActionInterval)
            {
                return true;
            }
            var dwActionIntervalTime = MDwActionIntervalTime;
            switch (wIdent)
            {
                case Grobal2.CM_LONGHIT:
                    if (M2Share.Config.boControlRunLongHit && MWOldIdent == Grobal2.CM_RUN && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_HIT:
                    if (M2Share.Config.boControlWalkHit && MWOldIdent == Grobal2.CM_WALK && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwWalkHitIntervalTime; // 走位攻击
                    }
                    if (M2Share.Config.boControlRunHit && MWOldIdent == Grobal2.CM_RUN && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunHitIntervalTime;// 跑位攻击
                    }
                    break;
                case Grobal2.CM_RUN:
                    if (M2Share.Config.boControlRunLongHit && MWOldIdent == Grobal2.CM_LONGHIT && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    if (M2Share.Config.boControlRunHit && MWOldIdent == Grobal2.CM_HIT && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunHitIntervalTime;// 跑位攻击
                    }
                    if (M2Share.Config.boControlRunMagic && MWOldIdent == Grobal2.CM_SPELL && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
                case Grobal2.CM_WALK:
                    if (M2Share.Config.boControlWalkHit && MWOldIdent == Grobal2.CM_HIT && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwWalkHitIntervalTime;// 走位攻击
                    }
                    if (M2Share.Config.boControlRunLongHit && MWOldIdent == Grobal2.CM_LONGHIT && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Grobal2.CM_SPELL:
                    if (M2Share.Config.boControlRunMagic && MWOldIdent == Grobal2.CM_RUN && MBtOldDir != Direction)
                    {
                        dwActionIntervalTime = MDwRunMagicIntervalTime;// 跑位魔法
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
                MDwActionTick = HUtil32.GetTickCount();
                result = true;
            }
            else
            {
                dwDelayTime = dwActionIntervalTime - dwCheckTime;
            }
            MWOldIdent = wIdent;
            MBtOldDir = Direction;
            return result;
        }

        public void SetScriptLabel(string sLabel)
        {
            CanJmpScriptLableMap.Clear();
            CanJmpScriptLableMap.Add(sLabel, sLabel);
        }

        /// <summary>
        /// 取得当前脚本可以跳转的标签
        /// </summary>
        /// <param name="sMsg"></param>
        public void GetScriptLabel(string sMsg)
        {
            var sText = string.Empty;
            CanJmpScriptLableMap.Clear();
            while (true)
            {
                if (string.IsNullOrEmpty(sMsg))
                {
                    break;
                }
                sMsg = HUtil32.GetValidStr3(sMsg, ref sText, '\\');
                if (string.IsNullOrEmpty(sText)) continue;
                var matchs = M2Share.MatchScriptLabel(sText);
                if (matchs.Count <= 0) continue;
                foreach (Match item in matchs)
                {
                    var sCmdStr = item.Value;
                    var sLabel = HUtil32.GetValidStr3(sCmdStr, ref sCmdStr, HUtil32.Backslash);
                    if (!string.IsNullOrEmpty(sLabel) && !CanJmpScriptLableMap.ContainsKey(sLabel))
                    {
                        CanJmpScriptLableMap.Add(sLabel, sLabel);
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
            if (CanJmpScriptLableMap.ContainsKey(sLabel))
            {
                return true;
            }
            if (string.Compare(sLabel, MSPlayDiceLabel, StringComparison.OrdinalIgnoreCase) == 0)
            {
                MSPlayDiceLabel = string.Empty;
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
                    if (MNMemberType == 0)
                    {
                        result = false;
                    }
                    break;
                case 81:
                    if (MNMemberType != HUtil32.LoWord(stdItem.NeedLevel) || MNMemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
                case 82:
                    if (MNMemberType < HUtil32.LoWord(stdItem.NeedLevel) || MNMemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
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
                var loadList = new StringList();
                loadList.LoadFromFile(sUnMarryFileName);
                for (var i = 0; i < loadList.Count; i++)
                {
                    if (string.Compare(loadList[i], this.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        loadList.RemoveAt(i);
                        boIsfound = true;
                        break;
                    }
                }
                loadList.SaveToFile(sUnMarryFileName);
                loadList.Dispose();
            }
            if (boIsfound)
            {
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMarryManLoginMsg, MSDearName, MSDearName);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMarryWoManLoginMsg, ChrName, ChrName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                MSDearName = "";
                RefShowName();
            }
            MDearHuman = M2Share.WorldEngine.GetPlayObject(MSDearName);
            if (MDearHuman != null)
            {
                MDearHuman.MDearHuman = this;
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineSelfMsg, MSDearName, ChrName, MDearHuman.Envir.MapDesc, MDearHuman.CurrX, MDearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sManLoginDearOnlineDearMsg, MSDearName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    MDearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineSelfMsg, MSDearName, ChrName, MDearHuman.Envir.MapDesc, MDearHuman.CurrX, MDearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sWoManLoginDearOnlineDearMsg, MSDearName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    MDearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
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
                if (string.Compare(M2Share.g_UnForceMasterList[i], this.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.g_UnForceMasterList.RemoveAt(i);
                    M2Share.SaveUnForceMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound)
            {
                if (MBoMaster)
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMasterLoginMsg, MSMasterName);
                }
                else
                {
                    sSayMsg = string.Format(M2Share.g_sfUnMasterListLoginMsg, MSMasterName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                MSMasterName = "";
                RefShowName();
            }
            if (!string.IsNullOrEmpty(MSMasterName) && !MBoMaster)
            {
                if (Abil.Level >= M2Share.Config.MasterOKLevel)
                {
                    var human = M2Share.WorldEngine.GetPlayObject(MSMasterName);
                    if (human != null && !human.Death && !human.Ghost)
                    {
                        sSayMsg = string.Format(M2Share.g_sYourMasterListUnMasterOKMsg, ChrName);
                        human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        if (ChrName == human.MSMasterName)// 如果大徒弟则将师父上的名字去掉
                        {
                            human.MSMasterName = "";
                            human.RefShowName();
                        }
                        for (var i = 0; i < human.MMasterList.Count; i++)
                        {
                            if (human.MMasterList[i] == this)
                            {
                                human.MMasterList.RemoveAt(i);
                                break;
                            }
                        }
                        MSMasterName = "";
                        RefShowName();
                        if (human.MBtCreditPoint + M2Share.Config.MasterOKCreditPoint <= byte.MaxValue)
                        {
                            human.MBtCreditPoint += (byte)M2Share.Config.MasterOKCreditPoint;
                        }
                        human.BonusPoint += M2Share.Config.nMasterOKBonusPoint;
                        human.SendMsg(human, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        // 如果师父不在线则保存到记录表中
                        boIsfound = false;
                        for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
                        {
                            if (string.Compare(M2Share.g_UnMasterList[i], this.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                boIsfound = true;
                                break;
                            }
                        }
                        if (!boIsfound)
                        {
                            M2Share.g_UnMasterList.Add(MSMasterName);
                        }
                        if (!boIsfound)
                        {
                            M2Share.SaveUnMasterList();
                        }
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        MSMasterName = "";
                        RefShowName();
                    }
                }
            }
            // 处理出师记录
            boIsfound = false;
            for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
            {
                if (string.Compare(M2Share.g_UnMasterList[i], this.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.g_UnMasterList.RemoveAt(i);
                    M2Share.SaveUnMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound && MBoMaster)
            {
                SysMsg(M2Share.g_sUnMasterLoginMsg, MsgColor.Red, MsgType.Hint);
                MSMasterName = "";
                RefShowName();
                if (MBtCreditPoint + M2Share.Config.MasterOKCreditPoint <= byte.MaxValue)
                {
                    MBtCreditPoint += (byte)M2Share.Config.MasterOKCreditPoint;
                }
                BonusPoint += M2Share.Config.nMasterOKBonusPoint;
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            }
            if (string.IsNullOrEmpty(MSMasterName))
            {
                return;
            }
            if (MBoMaster) // 师父上线通知
            {
                MMasterHuman = M2Share.WorldEngine.GetPlayObject(MSMasterName);
                if (MMasterHuman != null)
                {
                    MMasterHuman.MMasterHuman = this;
                    MMasterList.Add(MMasterHuman);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineSelfMsg, MSMasterName, ChrName, MMasterHuman.Envir.MapDesc, MMasterHuman.CurrX, MMasterHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(M2Share.g_sMasterOnlineMasterListMsg, MSMasterName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    MMasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    SysMsg(M2Share.g_sMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                // 徒弟上线通知
                if (!string.IsNullOrEmpty(MSMasterName))
                {
                    MMasterHuman = M2Share.WorldEngine.GetPlayObject(MSMasterName);
                    if (MMasterHuman != null)
                    {
                        if (MMasterHuman.MSMasterName == ChrName)
                        {
                            MMasterHuman.MMasterHuman = this;
                        }
                        MMasterHuman.MMasterList.Add(this);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineSelfMsg, MSMasterName, ChrName, MMasterHuman.Envir.MapDesc, MMasterHuman.CurrX, MMasterHuman.CurrY);
                        SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                        sSayMsg = string.Format(M2Share.g_sMasterListOnlineMasterMsg, MSMasterName, ChrName, Envir.MapDesc, CurrX, CurrY);
                        MMasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
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
            sMyInfo = sMyInfo.Replace("%name", ChrName);
            sMyInfo = sMyInfo.Replace("%map", Envir.MapDesc);
            sMyInfo = sMyInfo.Replace("%x", CurrX.ToString());
            sMyInfo = sMyInfo.Replace("%y", CurrY.ToString());
            sMyInfo = sMyInfo.Replace("%level", Abil.Level.ToString());
            sMyInfo = sMyInfo.Replace("%gold", Gold.ToString());
            sMyInfo = sMyInfo.Replace("%pk", PkPoint.ToString());
            sMyInfo = sMyInfo.Replace("%minhp", WAbil.HP.ToString());
            sMyInfo = sMyInfo.Replace("%maxhp", WAbil.MaxHP.ToString());
            sMyInfo = sMyInfo.Replace("%minmp", WAbil.MP.ToString());
            sMyInfo = sMyInfo.Replace("%maxmp", WAbil.MaxMP.ToString());
            sMyInfo = sMyInfo.Replace("%mindc", HUtil32.LoWord(WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%maxdc", HUtil32.HiWord(WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%minmc", HUtil32.LoWord(WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%maxmc", HUtil32.HiWord(WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%minsc", HUtil32.LoWord(WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%maxsc", HUtil32.HiWord(WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%logontime", MDLogonTime.ToString());
            sMyInfo = sMyInfo.Replace("%logonint", ((HUtil32.GetTickCount() - MDwLogonTick) / 60000).ToString());
            return sMyInfo;
        }

        private bool CheckItemBindUse(UserItem userItem)
        {
            TItemBind itemBind;
            var result = true;
            for (var i = 0; i < M2Share.g_ItemBindAccount.Count; i++)
            {
                itemBind = M2Share.g_ItemBindAccount[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, UserAccount, StringComparison.OrdinalIgnoreCase) == 0)
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
                itemBind = M2Share.g_ItemBindIPaddr[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, LoginIpAddr, StringComparison.OrdinalIgnoreCase) == 0)
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
            for (var i = 0; i < M2Share.g_ItemBindChrName.Count; i++)
            {
                itemBind = M2Share.g_ItemBindChrName[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sItemIsNotThisChrName, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            return result;
        }

        private void ProcessClientPassword(ProcessMessage processMsg)
        {
            if (processMsg.wParam == 0)
            {
                ProcessUserLineMsg("@" + CommandMgr.Commands.Unlock.CmdName);
                return;
            }
            var sData = processMsg.Msg;
            var nLen = sData.Length;
            if (MBoSetStoragePwd)
            {
                MBoSetStoragePwd = false;
                if (nLen > 3 && nLen < 8)
                {
                    MSTempPwd = sData;
                    MBoReConfigPwd = true;
                    SysMsg(M2Share.g_sReSetPasswordMsg, MsgColor.Green, MsgType.Hint);// '请重复输入一次仓库密码：'
                    SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                }
                else
                {
                    SysMsg(M2Share.g_sPasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                }
                return;
            }
            if (MBoReConfigPwd)
            {
                MBoReConfigPwd = false;
                if (string.Compare(MSTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    MSStoragePwd = sData;
                    MBoPasswordLocked = true;
                    MSTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);// '密码设置成功!!，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。'
                }
                else
                {
                    MSTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (MBoUnLockPwd || MBoUnLockStoragePwd)
            {
                if (string.Compare(MSStoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    MBoPasswordLocked = false;
                    if (MBoUnLockPwd)
                    {
                        if (M2Share.Config.LockDealAction)
                        {
                            MBoCanDeal = true;
                        }
                        if (M2Share.Config.LockDropAction)
                        {
                            MBoCanDrop = true;
                        }
                        if (M2Share.Config.LockWalkAction)
                        {
                            MBoCanWalk = true;
                        }
                        if (M2Share.Config.LockRunAction)
                        {
                            MBoCanRun = true;
                        }
                        if (M2Share.Config.LockHitAction)
                        {
                            MBoCanHit = true;
                        }
                        if (M2Share.Config.LockSpellAction)
                        {
                            MBoCanSpell = true;
                        }
                        if (M2Share.Config.LockSendMsgAction)
                        {
                            MBoCanSendMsg = true;
                        }
                        if (M2Share.Config.LockUserItemAction)
                        {
                            MBoCanUseItem = true;
                        }
                        if (M2Share.Config.LockInObModeAction)
                        {
                            ObMode = false;
                            AdminMode = false;
                        }
                        MBoLockLogoned = true;
                        SysMsg(M2Share.g_sPasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    if (MBoUnLockStoragePwd)
                    {
                        if (M2Share.Config.LockGetBackItemAction)
                        {
                            MBoCanGetBackItem = true;
                        }
                        SysMsg(M2Share.g_sStorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                }
                else
                {
                    MBtPwdFailCount++;
                    SysMsg(M2Share.g_sUnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                    if (MBtPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                    }
                }
                MBoUnLockPwd = false;
                MBoUnLockStoragePwd = false;
                return;
            }
            if (MBoCheckOldPwd)
            {
                MBoCheckOldPwd = false;
                if (MSStoragePwd == sData)
                {
                    SendMsg(this, Grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sSetPasswordMsg, MsgColor.Green, MsgType.Hint);
                    MBoSetStoragePwd = true;
                }
                else
                {
                    MBtPwdFailCount++;
                    SysMsg(M2Share.g_sOldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                    if (MBtPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                        MBoPasswordLocked = true;
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
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (playObject != null)
            {
                if (GetFrontPosition(ref nX, ref nY))
                {
                    if (GetRecallXy(nX, nY, 3, ref n18, ref n1C))
                    {
                        playObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        playObject.SpaceMove(MapName, n18, n1C, 0);
                    }
                }
                else
                {
                    SysMsg("召唤失败!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                SysMsg(Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
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
            var guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (guild == null)
            {
                SysMsg("行会不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var boReQuestOk = false;
            var warGuild = MyGuild.AddWarGuild(guild);
            if (warGuild != null)
            {
                if (guild.AddWarGuild(MyGuild) == null)
                {
                    warGuild.dwWarTick = 0;
                }
                else
                {
                    boReQuestOk = true;
                }
            }
            if (boReQuestOk)
            {
                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, guild.sGuildName);
            }
        }

        private bool CheckDenyLogon()
        {
            var result = false;
            if (M2Share.GetDenyIPAddrList(LoginIpAddr))
            {
                SysMsg(M2Share.g_sYourIPaddrDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyAccountList(UserAccount))
            {
                SysMsg(M2Share.g_sYourAccountDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyChrNameList(ChrName))
            {
                SysMsg(M2Share.g_sYourChrNameDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            if (result)
            {
                MBoEmergencyClose = true;
            }
            return result;
        }

        /// <summary>
        /// 转移到指定位面服务器
        /// </summary>
        public void ChangePlanesServer(string serveraddr, int gamePort)
        {
            this.SendMsg(this, Grobal2.RM_RECONNECTION, 0, 0, 0, 0, serveraddr + '/' + gamePort);
        }

        private void ProcessQueryValue(int npc, string sData)
        {
            NormNpc normNpc;
            var sRefMsg = string.Empty;
            if (!Ghost && !string.IsNullOrEmpty(MSGotoNpcLabel))
            {
                sRefMsg = EDCode.DeCodeString(sData);
                //if (IsInGuildRankNameFilterList(sRefMsg))
                //{
                //    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, sIsInQVFilterListMsg);
                //    return;
                //}
            }
            switch (MBtValType)
            {
                case 0:
                    MNSval[MBtValLabel] = sRefMsg;
                    break;
                case 1:
                    MNMval[MBtValLabel] = HUtil32.StrToInt(sRefMsg, 0);
                    break;
            }
            switch (MBtValNpcType)
            {
                case 0:
                    normNpc = WorldServer.FindMerchant<Merchant>(npc);
                    if (normNpc == null)
                    {
                        normNpc = WorldServer.FindNpc<NormNpc>(npc);
                    }
                    if (normNpc != null)
                    {
                        if (normNpc.Envir == Envir && Math.Abs(normNpc.CurrX - CurrX) <= 15 && Math.Abs(normNpc.CurrY - CurrY) <= 15)
                        {
                            normNpc.GotoLable(this, MSGotoNpcLabel, false);
                        }
                    }
                    break;
                case 1:
                    if (M2Share.g_FunctionNPC != null)
                    {
                        M2Share.g_FunctionNPC.GotoLable(this, MSGotoNpcLabel, false);
                    }
                    break;
                case 2:
                    if (M2Share.g_ManageNPC != null)
                    {
                        M2Share.g_ManageNPC.GotoLable(this, MSGotoNpcLabel, false);
                    }
                    break;
            }
            MSGotoNpcLabel = string.Empty;
        }

        private void ClientMerchantItemDlgSelect(int nParam1, ushort nParam2, ushort nParam3)
        {
            DlgItemIndex = 0;
            if (!Death && !Ghost)
            {
                if (nParam1 == 0)
                {
                    return;
                }
                NormNpc npc = WorldServer.FindMerchant<Merchant>(nParam1);
                if (npc == null)
                {
                    npc = WorldServer.FindNpc<NormNpc>(nParam1);
                }
                if (npc != null)
                {
                    //LastNPC = Npc;
                }
                if (npc == null)
                {
                    return;
                }
                if (npc.Envir == Envir && Math.Abs(npc.CurrX - CurrX) < 15 && Math.Abs(npc.CurrY - CurrY) < 15 || npc.m_boIsHide)
                {
                    DlgItemIndex = HUtil32.MakeLong(nParam2, nParam3);
                    int nTemp;
                    if (TakeDlgItem && DlgItemIndex > 0)
                    {
                        nTemp = 255;
                        for (var i = 0; i < ItemList.Count; i++)
                        {
                            var userItem = ItemList[i];
                            if (userItem.Index == DlgItemIndex)
                            {
                                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                                if (stdItem != null)
                                {
                                    if (stdItem.NeedIdentify == 1)
                                    {
                                        // M2Share.ItemEventSource.AddGameLog('10' + #9 + m_sMapName + #9 +inttostr(m_nCurrX) + #9 + inttostr(m_nCurrY) + #9 +m_sChrName + #9 + StdItem.Name + #9 +inttostr(UserItem.MakeIndex) + #9 + '1' + #9 + m_sChrName);
                                        SendDelItems(userItem);
                                        ItemList.RemoveAt(i);
                                        DlgItemIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        nTemp = 0;
                    }
                    SendDefMessage(Grobal2.SM_ITEMDLGSELECT, 1, nTemp, 0, 0, "");
                    //Npc.m_OprCount = 0;
                    npc.GotoLable(this, MSGotoNpcLabel, false);
                    MSGotoNpcLabel = string.Empty;
                }
            }
        }

    }
}