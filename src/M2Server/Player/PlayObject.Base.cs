using M2Server.Actor;
using M2Server.Event.Events;
using M2Server.Items;
using M2Server.Magic;
using M2Server.Monster;
using System.Text.RegularExpressions;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Player
{
    public partial class PlayObject
    {
        private bool ClientPickUpItemIsSelf(int actorId)
        {
            bool result;
            if (actorId == 0 || ActorId == actorId)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ClientPickUpItemIsOfGroup(int actorId)
        {
            if (GroupOwner == 0)
            {
                return false;
            }
            IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
            for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
            {
                if (groupOwnerPlay.GroupMembers[i].ActorId == actorId)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClientPickUpItem()
        {
            if (Dealing)
            {
                return;
            }
            MapItem mapItem = default;
            var success = Envir.GetItem(CurrX, CurrY, ref mapItem);
            if (!success)
            {
                return;
            }
            if (HUtil32.GetTickCount() - mapItem.CanPickUpTick > SystemShare.Config.FloorItemCanPickUpTime)// 2 * 60 * 1000
            {
                mapItem.OfBaseObject = 0;
            }
            if (!ClientPickUpItemIsSelf(mapItem.OfBaseObject) && !ClientPickUpItemIsOfGroup(mapItem.OfBaseObject))
            {
                SysMsg(Settings.CanotPickUpItem, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (mapItem.Name.Equals(Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase))
            {
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.Item, mapItem.ItemId, null) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, CurrX, CurrY, "");
                        if (M2Share.GameLogGold)
                        {
                            // M2Share.EventSource.AddEventLog(4, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.StringGoldName + "\t" + mapItem.Count + "\t" + '1' + "\t" + '0');
                        }
                        GoldChanged();
                        Dispose(mapItem);
                    }
                    else
                    {
                        Envir.AddItemToMap(CurrX, CurrY, mapItem);
                    }
                }
                return;
            }
            if (IsEnoughBag())
            {
                if (Envir.DeleteFromMap(CurrX, CurrY, CellType.Item, mapItem.ItemId, null) == 1)
                {
                    UserItem userItem = mapItem.UserItem;
                    StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                    if (stdItem != null && IsAddWeightAvailable(SystemShare.ItemSystem.GetStdItemWeight(userItem.Index)))
                    {
                        SendMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, CurrX, CurrY);
                        AddItemToBag(userItem);
                        if (!M2Share.IsCheapStuff(stdItem.StdMode))
                        {
                            if (stdItem.NeedIdentify == 1)
                            {
                                // M2Share.EventSource.AddEventLog(4, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name+ "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        Dispose(mapItem);
                        if (Race == ActorRace.Play)
                        {
                            SendAddItem(userItem);
                        }
                    }
                    else
                    {
                        Dispose(userItem);
                        Envir.AddItemToMap(CurrX, CurrY, mapItem);
                    }
                }
            }
        }

        public void WinExp(int dwExp)
        {
            if (Abil.Level > SystemShare.Config.LimitExpLevel)
            {
                dwExp = SystemShare.Config.LimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = SystemShare.Config.KillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = MNKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(KillMonExpRate / 100.0 * dwExp);// 人物指定的杀怪经验倍数
                if (Envir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(Envir.Flag.ExpRate / 100.0 * dwExp);// 地图上指定杀怪经验倍数
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(Messages.RM_WINEXP, 0, dwExp, 0, 0);
            if (Abil.Exp >= Abil.MaxExp)
            {
                Abil.Exp -= Abil.MaxExp;
                if (Abil.Level < Settings.MAXUPLEVEL)
                {
                    Abil.Level++;
                }
                HasLevelUp(Abil.Level - 1);
                AddBodyLuck(100);
                //M2Share.EventSource.AddEventLog(12, MapName + "\t" + Abil.Level + "\t" + Abil.Exp + "\t" + ChrName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public bool IncGold(int tGold)
        {
            bool result = false;
            if (Gold + tGold <= SystemShare.Config.HumanMaxGold)
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
            return ItemList.Count < Grobal2.MaxBagItem;
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
            StdItem item = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (item == null)
            {
                return;
            }
            ClientItem clientItem = new ClientItem();
            SystemShare.ItemSystem.GetUpgradeStdItem(item, userItem, ref clientItem);
            clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
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
            ClientMsg = Messages.MakeMessage(Messages.SM_ADDITEM, ActorId, 0, 0, 1);
            SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
        }

        public bool IsBlockWhisper(string sName)
        {
            bool result = false;
            for (int i = 0; i < LockWhisperList.Count; i++)
            {
                if (string.Compare(sName, LockWhisperList[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private byte DayBright()
        {
            byte result;
            if (Envir.Flag.DayLight)
            {
                return 0;
            }
            if (Envir.Flag.boDarkness)
            {
                return 1;
            }
            switch (Bright)
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
            int n8 = 0;
            if (Envir.Flag.FightZone)
            {
                n8 = n8 | 1;
            }
            if (Envir.Flag.SafeArea)
            {
                n8 = n8 | 2;
            }
            if (InGuildWarArea)
            {
                n8 = n8 | 4;
            }
            SendDefMessage(Messages.SM_AREASTATE, n8, 0, 0, 0);
        }

        public void RefMyStatus()
        {
            RecalcAbilitys();
            SendMsg(Messages.RM_MYSTATUS, 0, 0, 0, 0);
        }

        /// <summary>
        /// 祈祷套装生效
        /// </summary>
        private void ProcessSpiritSuite()
        {
            if (!SystemShare.Config.SpiritMutiny || !IsSpirit)
            {
                return;
            }
            IsSpirit = false;
            for (int i = 0; i < UseItems.Length; i++)
            {
                UserItem useItem = UseItems[i];
                if (useItem == null)
                {
                    continue;
                }
                if (useItem.Index <= 0)
                {
                    continue;
                }
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(useItem.Index);
                if (stdItem != null)
                {
                    if (stdItem.Shape is ItemShapeConst.SpiritItem3 or ItemShapeConst.SpiritItem1 or ItemShapeConst.SpiritItem2 or ItemShapeConst.SpiritItem4)
                    {
                        SendDelItems(useItem);
                        useItem.Index = 0;
                    }
                }
            }
            RecalcAbilitys();
            M2Share.SpiritMutinyTick = HUtil32.GetTickCount() + SystemShare.Config.SpiritMutinyTime;
            SystemShare.WorldEngine.SendBroadCastMsg("神之祈祷，天地震怒，尸横遍野...", MsgType.System);
            SysMsg("祈祷发出强烈的宇宙效应", MsgColor.Green, MsgType.Hint);
        }

        private void ProcessSlaveMutiny()
        {
            // 宝宝在主人死亡后死亡处理
            if (!Death)
            {
                return;
            }
            if (!SystemShare.Config.MasterDieMutiny)
            {
                return;
            }
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (SlaveList[i].Master.LastHiter != null && (M2Share.RandomNumber.Random(SystemShare.Config.MasterDieMutinyRate) == 0))
                {
                    SlaveList[i].Master = null;
                    SlaveList[i].SlaveExpLevel = (byte)SystemShare.Config.SlaveColor.Length;
                    SlaveList[i].RecalcAbilitys();
                    SlaveList[i].WAbil.DC = (ushort)HUtil32.MakeLong((short)(HUtil32.LoByte(SlaveList[i].WAbil.DC) * SystemShare.Config.MasterDieMutinyPower), (short)(HUtil32.HiByte(SlaveList[i].WAbil.DC) * SystemShare.Config.MasterDieMutinyPower));
                    SlaveList[i].WalkSpeed = SlaveList[i].WalkSpeed / SystemShare.Config.MasterDieMutinySpeed;
                    SlaveList[i].RefNameColor();
                    SlaveList[i].RefShowName();
                }
                else
                {
                    SlaveList[i].WAbil.HP = 0;
                }
            }
        }

        private void ProcessSlaveGhost()
        {
            if (!Ghost)
            {
                return;
            }
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                SlaveList[i].MakeGhost();
            }
        }

        private void LogonTimcCost()
        {
            int n08;
            if (PayMent == 2 || SystemShare.Config.TestServer)
            {
                n08 = (HUtil32.GetTickCount() - LogonTick) / 1000;
            }
            else
            {
                n08 = 0;
            }
            string sC = LoginIpAddr + "\t" + UserAccount + "\t" + ChrName + "\t" + n08 + "\t" + DateTimeOffset.FromUnixTimeMilliseconds(LogonTime).ToString("yyyy-mm-dd hh:mm:ss") + "\t" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + PayMode;
            M2Share.AddLogonCostLog(sC);
            if (PayMode == 2)
            {
                //IdSrvClient.Instance.SendLogonCostMsg(UserAccount, n08 / 60);
            }
        }

        private bool RunTo(byte btDir, bool boFlag, int nDestX, int nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            bool result = false;
            try
            {
                var nOldX = CurrX;
                var nOldY = CurrY;
                Dir = btDir;
                bool canWalk = SystemShare.Config.DiableHumanRun || Permission > 9 && SystemShare.Config.boGMRunAll;
                switch (btDir)
                {
                    case Direction.Up:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true))
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Direction.UpRight:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true))
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Direction.Right:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true))
                        {
                            CurrX += 2;
                        }
                        break;
                    case Direction.DownRight:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 &&
                            Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) &&
                            Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true))
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Direction.Down:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true))
                        {
                            CurrY += 2;
                        }
                        break;
                    case Direction.DownLeft:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true))
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }
                        break;
                    case Direction.Left:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true))
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Direction.UpLeft:
                        if (CurrX > 1 && CurrY > 1 &&
                            Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true))
                        {
                            CurrX -= 2;
                            CurrY -= 2;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Messages.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = nOldX;
                        CurrY = nOldY;
                        Envir.MoveToMovingObject(nOldX, nOldY, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
            return result;
        }

        private bool HorseRunTo(byte btDir, bool boFlag)
        {
            const string sExceptionMsg = "[Exception] PlayObject::HorseRunTo";
            bool result = false;
            try
            {
                var nOldX = CurrX;
                var nOldY = CurrY;
                Dir = btDir;
                bool canWalk = SystemShare.Config.DiableHumanRun || Permission > 9 && SystemShare.Config.boGMRunAll;
                switch (btDir)
                {
                    case Direction.Up:
                        if (CurrY > 2 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 3, true))
                        {
                            CurrY -= 3;
                        }
                        break;
                    case Direction.UpRight:
                        if (CurrX < Envir.Width - 3 && CurrY > 2 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY - 3, true))
                        {
                            CurrX += 3;
                            CurrY -= 3;
                        }
                        break;
                    case Direction.Right:
                        if (CurrX < Envir.Width - 3 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY, true))
                        {
                            CurrX += 3;
                        }
                        break;
                    case Direction.DownRight:
                        if (CurrX < Envir.Width - 3 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX + 3, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 3, CurrY + 3, true))
                        {
                            CurrX += 3;
                            CurrY += 3;
                        }
                        break;
                    case Direction.Down:
                        if (CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 3, true))
                        {
                            CurrY += 3;
                        }
                        break;
                    case Direction.DownLeft:
                        if (CurrX > 2 && CurrY < Envir.Height - 3 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY + 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY + 3, true))
                        {
                            CurrX -= 3;
                            CurrY += 3;
                        }
                        break;
                    case Direction.Left:
                        if (CurrX > 2 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY, true))
                        {
                            CurrX -= 3;
                        }
                        break;
                    case Direction.UpLeft:
                        if (CurrX > 2 && CurrY > 2 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.CanWalkEx(CurrX - 3, CurrY - 3, canWalk) &&
                            Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 3, CurrY - 3, true))
                        {
                            CurrX -= 3;
                            CurrY -= 3;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Messages.RM_HORSERUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = nOldX;
                        CurrY = nOldY;
                        Envir.MoveToMovingObject(nOldX, nOldY, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
            return result;
        }

        protected void ThrustingOnOff(bool boSwitch)
        {
            UseThrusting = boSwitch;
            if (UseThrusting)
            {
                SysMsg(Settings.ThrustingOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(Settings.ThrustingOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void HalfMoonOnOff(bool boSwitch)
        {
            UseHalfMoon = boSwitch;
            if (UseHalfMoon)
            {
                SysMsg(Settings.HalfMoonOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(Settings.HalfMoonOff, MsgColor.Green, MsgType.Hint);
            }
        }

        private void RedHalfMoonOnOff(bool boSwitch)
        {
            RedUseHalfMoon = boSwitch;
            if (RedUseHalfMoon)
            {
                SysMsg(Settings.RedHalfMoonOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(Settings.RedHalfMoonOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void SkillCrsOnOff(bool boSwitch)
        {
            CrsHitkill = boSwitch;
            if (CrsHitkill)
            {
                SysMsg(Settings.CrsHitOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(Settings.CrsHitOff, MsgColor.Green, MsgType.Hint);
            }
        }

        protected void SkillTwinOnOff(bool boSwitch)
        {
            TwinHitSkill = boSwitch;
            if (TwinHitSkill)
            {
                SysMsg(Settings.TwinHitOn, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                SysMsg(Settings.TwinHitOff, MsgColor.Green, MsgType.Hint);
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
                SysMsg(Settings.FireSpiritsSummoned, MsgColor.Green, MsgType.Hint);
                return true;
            }
            SysMsg(Settings.FireSpiritsFail, MsgColor.Red, MsgType.Hint);
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
            if (!IsCanDeal)
            {
                SendMsg(SystemShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotTryDealMsg);
                return;
            }
            if (Death || Ghost)
            {
                return;
            }
            if (HUtil32.GetTickCount() - ClickNpcTime > SystemShare.Config.ClickNpcTime)
            {
                ClickNpcTime = HUtil32.GetTickCount();
                INormNpc normNpc = SystemShare.WorldEngine.FindMerchant(actorId) ?? SystemShare.WorldEngine.FindNpc(actorId);
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
            return SystemShare.WorldEngine.GetMapOfRangeHumanCount(Envir, CurrX, CurrY, 10);
        }

        private void GetStartPoint()
        {
            if (PvpLevel() >= 2)
            {
                HomeMap = SystemShare.Config.RedHomeMap;
                HomeX = SystemShare.Config.RedHomeX;
                HomeY = SystemShare.Config.RedHomeY;
                return;
            }
            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (string.Compare(M2Share.StartPointList[i].MapName, Envir.MapName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    HomeMap = M2Share.StartPointList[i].MapName;
                    HomeX = M2Share.StartPointList[i].CurrX;
                    HomeY = M2Share.StartPointList[i].CurrY;
                    break;
                }
            }
        }

        private static void MobPlace(string sX, string sY, string sMonName, string sCount)
        {

        }

        public void DealCancel()
        {
            if (!Dealing)
            {
                return;
            }
            Dealing = false;
            SendDefMessage(Messages.SM_DEALCANCEL, 0, 0, 0, 0);
            if (DealCreat != null)
            {
                DealCreat.DealCancel();
            }
            DealCreat = null;
            GetBackDealItems();
            SysMsg(Settings.DealActionCancelMsg, MsgColor.Green, MsgType.Hint);
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
            IActor playObject;
            const string sExceptionMsg = "[Exception] PlayObject::GainExp";
            double[] bonus = { 1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2 };
            try
            {
                if (GroupOwner != 0)
                {
                    int sumlv = 0;
                    int n = 0;
                    IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
                    for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                    {
                        playObject = groupOwnerPlay.GroupMembers[i];
                        if (!playObject.Death && Envir == playObject.Envir && Math.Abs(CurrX - playObject.CurrX) <= 12 && Math.Abs(CurrX - playObject.CurrX) <= 12)
                        {
                            sumlv = sumlv + playObject.Abil.Level;
                            n++;
                        }
                    }

                    if (sumlv > 0 && n > 1)
                    {
                        if (n >= 0 && n <= Grobal2.GroupMax)
                        {
                            dwExp = HUtil32.Round(dwExp * bonus[n]);
                        }
                        for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                        {
                            playObject = groupOwnerPlay.GroupMembers[i];
                            if (!playObject.Death && Envir == playObject.Envir && Math.Abs(CurrX - playObject.CurrX) <= 12 && Math.Abs(CurrX - playObject.CurrX) <= 12)
                            {
                                if (SystemShare.Config.HighLevelKillMonFixExp)
                                {
                                    playObject.WinExp(HUtil32.Round(dwExp / (double)n)); // 在高等级经验不变时，把组队的经验平均分配
                                }
                                else
                                {
                                    playObject.WinExp(HUtil32.Round(dwExp / (double)sumlv * playObject.Abil.Level));
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
                M2Share.Logger.Error(sExceptionMsg);
            }
        }

        public void GameTimeChanged()
        {
            if (Bright != M2Share.GameTime)
            {
                Bright = M2Share.GameTime;
                SendMsg(Messages.RM_DAYCHANGING, 0, 0, 0, 0);
            }
        }

        private void GetBackDealItems()
        {
            if (DealItemList.Count > 0)
            {
                for (int i = 0; i < DealItemList.Count; i++)
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
            return ChrName + " 标识:" + ActorId + " 权限等级: " + Permission + " 管理模式: " + HUtil32.BoolToStr(AdminMode)
                + " 隐身模式: " + HUtil32.BoolToStr(ObMode) + " 无敌模式: " + HUtil32.BoolToStr(SuperMan) + " 地图:" + MapName + '(' + Envir.MapDesc + ')'
                + " 座标:" + CurrX + ':' + CurrY + " 等级:" + Abil.Level + " 转生等级:" + ReLevel
                + " 经验:" + Abil.Exp + " 生命值: " + WAbil.HP + '-' + WAbil.MaxHP + " 魔法值: " + WAbil.MP + '-' + WAbil.MaxMP
                + " 攻击力: " + HUtil32.LoWord(WAbil.DC) + '-' + HUtil32.HiWord(WAbil.DC) + " 魔法力: " + HUtil32.LoWord(WAbil.MC) + '-'
                + HUtil32.HiWord(WAbil.MC) + " 道术: " + HUtil32.LoWord(WAbil.SC) + '-' + HUtil32.HiWord(WAbil.SC)
                + " 防御力: " + HUtil32.LoWord(WAbil.AC) + '-' + HUtil32.HiWord(WAbil.AC) + " 魔防力: " + HUtil32.LoWord(WAbil.MAC)
                + '-' + HUtil32.HiWord(WAbil.MAC) + " 准确:" + HitPoint + " 敏捷:" + SpeedPoint + " 速度:" + HitSpeed
                + " 仓库密码:" + StoragePwd + " 登录IP:" + LoginIpAddr + '(' + LoginIpLocal + ')' + " 登录帐号:" + UserAccount + " 登录时间:" + DateTimeOffset.FromUnixTimeMilliseconds(LogonTime).ToString("yyyy-MM-dd HH:mm:ss")
                + " 在线时长(分钟):" + (HUtil32.GetTickCount() - LogonTick) / 60000 + " 登录模式:" + PayMent + ' ' + SystemShare.Config.GameGoldName + ':' + GameGold
                + ' ' + SystemShare.Config.GamePointName + ':' + GamePoint + ' ' + SystemShare.Config.PayMentPointName + ':' + PayMentPoint + " 会员类型:" + MemberType
                + " 会员等级:" + MemberLevel + " 经验倍数:" + KillMonExpRate / 100 + " 攻击倍数:" + PowerRate / 100 + " 声望值:" + CreditPoint;
        }

        public void GoldChange(string sChrName, int nGold)
        {
            string s14;
            if (nGold > 0)
            {
                s14 = "增加完成";
            }
            else
            {
                s14 = "以删减";
            }
            SysMsg(sChrName + " 的金币 " + nGold + " 金币" + s14, MsgColor.Green, MsgType.Hint);
            if (M2Share.GameLogGold)
            {
                //M2Share.EventSource.AddEventLog(s10, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.StringGoldName + "\t" + nGold + "\t" + '1' + "\t" + sChrName);
            }
        }

        public void ClearStatusTime()
        {
            StatusTimeArr = new ushort[15];
        }

        private void SendMapDescription()
        {
            int nMusicid = -1;
            if (Envir.Flag.Music)
            {
                nMusicid = Envir.Flag.MusicId;
            }
            SendDefMessage(Messages.SM_MAPDESCRIPTION, nMusicid, 0, 0, 0, Envir.MapDesc);
        }

        public void ChangeSpaceMove(IEnvirnoment envir, short nX, short nY)
        {
            SwitchMapName = envir.MapName;
            SwitchMapX = nX;
            SwitchMapY = nY;
            SwitchData = true;
            ServerIndex = envir.ServerIndex;
            BoEmergencyClose = true;
            BoReconnection = true;
        }

        private void ReadAllBook()
        {
            //for (int i = 0; i < SystemShare.WorldEngine.MagicList.Count; i++)
            //{
            //    MagicInfo magic = SystemShare.WorldEngine.MagicList[i];
            //    UserMagic userMagic = new UserMagic
            //    {
            //        Magic = magic,
            //        MagIdx = magic.MagicId,
            //        Level = 2,
            //        Key = (char)0
            //    };
            //    userMagic.Level = 0;
            //    userMagic.TranPoint = 100000;
            //    MagicList.Add(userMagic);
            //    SendAddMagic(userMagic);
            //}
        }

        private void SendGoldInfo(bool boSendName)
        {
            string sMsg = string.Empty;
            if (SoftVersionDateEx == 0)
            {
                return;
            }
            if (boSendName)
            {
                sMsg = SystemShare.Config.GameGoldName + '\r' + SystemShare.Config.GamePointName;
            }
            SendDefMessage(Messages.SM_GAMEGOLDNAME, GameGold, HUtil32.LoWord(GamePoint), HUtil32.HiWord(GamePoint), 0, sMsg);
        }

        private void SendServerConfig()
        {
            if (SoftVersionDateEx == 0)
            {
                return;
            }

            if (SystemShare.Config.DiableHumanRun || Permission > 9 && SystemShare.Config.boGMRunAll)
            {
            }
            else
            {
                if (SystemShare.Config.boRunHuman || Envir.Flag.RunHuman)
                {
                }
                if (SystemShare.Config.boRunMon || Envir.Flag.RunMon)
                {
                }
                if (SystemShare.Config.boRunNpc)
                {
                }
                if (SystemShare.Config.boWarDisHumRun)
                {
                }
            }
            string sMsg = EDCode.EncodeMessage(SystemShare.Config.ClientConf);
            //var nRecog = HUtil32.MakeLong(HUtil32.MakeWord(nRunHuman, nRunMon), HUtil32.MakeWord(nRunNpc, nWarRunAll));
            //var nParam = (short)HUtil32.MakeWord(5, 0);
            SendDefMessage(Messages.SM_SERVERCONFIG, 0, 0, 0, 0, sMsg);
        }

        private void SendServerStatus()
        {
            if (Permission < 10)
            {
                return;
            }
            //this.SysMsg((HUtil32.CalcFileCRC(Application.ExeName)).ToString(), MsgColor.c_Red, MsgType.t_Hint);
        }

        // 检查角色的座标是否在指定误差范围以内
        // targetBaseObject 为要检查的角色，nX,nY 为比较的座标
        // 检查角色是否在指定座标的1x1 范围以内，如果在则返回True 否则返回 False
        protected bool CretInNearXy(IActor targeBaseObject, int nX, int nY)
        {
            if (Envir == null)
            {
                M2Share.Logger.Error("CretInNearXY nil PEnvir");
                return false;
            }
            for (int cX = nX - 1; cX <= nX + 1; cX++)
            {
                for (int cY = nY - 1; cY <= nY + 1; cY++)
                {
                    ref MapCellInfo cellInfo = ref Envir.GetCellInfo(cX, cY, out bool cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (int i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            CellObject cellObject = cellInfo.ObjList[i];
                            if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                            {
                                IActor baseObject = SystemShare.ActorMgr.Get(cellObject.CellObjId);
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

        public void SendUseItems()
        {
            string sSendMsg = string.Empty;
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    StdItem item = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                    if (item != null)
                    {
                        ClientItem clientItem = new ClientItem();
                        SystemShare.ItemSystem.GetUpgradeStdItem(item, UseItems[i], ref clientItem);
                        //Item.GetItemAddValue(UseItems[i], ref ClientItem.Item);
                        clientItem.Item.Name = CustomItemSystem.GetItemName(UseItems[i]);
                        clientItem.Dura = UseItems[i].Dura;
                        clientItem.DuraMax = UseItems[i].DuraMax;
                        clientItem.MakeIndex = UseItems[i].MakeIndex;
                        if (i == ItemLocation.Dress)
                        {
                            ChangeItemWithLevel(ref clientItem, Abil.Level);
                        }
                        ChangeItemByJob(ref clientItem, Abil.Level);
                        sSendMsg = sSendMsg + i + '/' + EDCode.EncodeBuffer(clientItem) + '/';
                    }
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                ClientMsg = Messages.MakeMessage(Messages.SM_SENDUSEITEMS, 0, 0, 0, 0);
                SendSocket(ClientMsg, sSendMsg);
            }
        }

        private void SendUseMagic()
        {
            string sSendMsg = string.Empty;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
                ClientMagic clientMagic = new ClientMagic();
                clientMagic.Key = userMagic.Key;
                clientMagic.Level = userMagic.Level;
                clientMagic.CurTrain = userMagic.TranPoint;
                clientMagic.Def = userMagic.Magic;
                sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientMagic) + '/';
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                ClientMsg = Messages.MakeMessage(Messages.SM_SENDMYMAGIC, 0, 0, 0, (short)MagicList.Count);
                SendSocket(ClientMsg, sSendMsg);
            }
        }

        private bool UseStdModeFunItem(StdItem stdItem)
        {
            bool result = false;
            if (SystemShare.FunctionNPC != null)
            {
                SystemShare.FunctionNPC.GotoLable(this, "@StdModeFunc" + stdItem.AniCount, false);
                result = true;
            }
            return result;
        }

        private static void RecalcAdjusBonusAdjustAb(byte abil, short val, ref short lov, ref short hiv)
        {
            byte lo = HUtil32.LoByte(abil);
            byte hi = HUtil32.HiByte(abil);
            lov = 0;
            hiv = 0;
            for (int i = 0; i < val; i++)
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
            NakedAbility bonusTick = default;
            NakedAbility nakedAbil = default;
            switch (Job)
            {
                case PlayJob.Warrior:
                    bonusTick = SystemShare.Config.BonusAbilofWarr;
                    nakedAbil = SystemShare.Config.NakedAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    bonusTick = SystemShare.Config.BonusAbilofWizard;
                    nakedAbil = SystemShare.Config.NakedAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    bonusTick = SystemShare.Config.BonusAbilofTaos;
                    nakedAbil = SystemShare.Config.NakedAbilofTaos;
                    break;
                default:
                    bonusTick = SystemShare.Config.BonusAbilofWarr;
                    nakedAbil = SystemShare.Config.NakedAbilofWarr;
                    break;
            }
            short adc = (short)(BonusAbil.DC / bonusTick.DC);
            short amc = (short)(BonusAbil.MC / bonusTick.MC);
            short asc = (short)(BonusAbil.SC / bonusTick.SC);
            short aac = (short)(BonusAbil.AC / bonusTick.AC);
            short amac = (short)(BonusAbil.MAC / bonusTick.MAC);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.DC, adc, ref ldc, ref hdc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.MC, amc, ref lmc, ref hmc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.SC, asc, ref lsc, ref hsc);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.AC, aac, ref lac, ref hac);
            RecalcAdjusBonusAdjustAb((byte)nakedAbil.MAC, amac, ref lmac, ref hmac);
            WAbil.DC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(WAbil.DC) + ldc), (ushort)(HUtil32.HiWord(WAbil.DC) + hdc));
            WAbil.MC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(WAbil.MC) + lmc), (ushort)(HUtil32.HiWord(WAbil.MC) + hmc));
            WAbil.SC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(WAbil.SC) + lsc), (ushort)(HUtil32.HiWord(WAbil.SC) + hsc));
            WAbil.AC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(WAbil.AC) + lac), (ushort)(HUtil32.HiWord(WAbil.AC) + hac));
            WAbil.MAC = (ushort)HUtil32.MakeLong((ushort)(HUtil32.LoWord(WAbil.MAC) + lmac), (ushort)(HUtil32.HiWord(WAbil.MAC) + hmac));
            WAbil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, WAbil.MaxHP + BonusAbil.HP / bonusTick.HP);
            WAbil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, WAbil.MaxMP + BonusAbil.MP / bonusTick.MP);
        }

        private void ClientAdjustBonus(int nPoint, string sMsg)
        {
            NakedAbility bonusAbil = new NakedAbility();
            int nTotleUsePoint = bonusAbil.DC + bonusAbil.MC + bonusAbil.SC + bonusAbil.AC + bonusAbil.MAC + bonusAbil.HP + bonusAbil.MP + bonusAbil.Hit + bonusAbil.Speed + bonusAbil.Reserved;
            if (nPoint + nTotleUsePoint == BonusPoint)
            {
                BonusPoint = nPoint;
                BonusAbil.DC += bonusAbil.DC;
                BonusAbil.MC += bonusAbil.MC;
                BonusAbil.SC += bonusAbil.SC;
                BonusAbil.AC += bonusAbil.AC;
                BonusAbil.MAC += bonusAbil.MAC;
                BonusAbil.HP += bonusAbil.HP;
                BonusAbil.MP += bonusAbil.MP;
                BonusAbil.Hit += bonusAbil.Hit;
                BonusAbil.Speed += bonusAbil.Speed;
                BonusAbil.Reserved += bonusAbil.Reserved;
                RecalcAbilitys();
                SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
            }
            else
            {
                SysMsg("非法数据调整!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        public int GetMyStatus()
        {
            int result = HungerStatus / 1000;
            if (result > 4)
            {
                result = 4;
            }
            return result;
        }

        private void SendAdjustBonus()
        {
            string sSendMsg = string.Empty;
            switch (Job)
            {
                case PlayJob.Warrior:
                    sSendMsg = EDCode.EncodeMessage(SystemShare.Config.BonusAbilofWarr) + '/' + EDCode.EncodeMessage(BonusAbil) + '/' + EDCode.EncodeMessage(SystemShare.Config.NakedAbilofWarr);
                    break;
                case PlayJob.Wizard:
                    sSendMsg = EDCode.EncodeMessage(SystemShare.Config.BonusAbilofWizard) + '/' + EDCode.EncodeMessage(BonusAbil) + '/' + EDCode.EncodeMessage(SystemShare.Config.NakedAbilofWizard);
                    break;
                case PlayJob.Taoist:
                    sSendMsg = EDCode.EncodeMessage(SystemShare.Config.BonusAbilofTaos) + '/' + EDCode.EncodeMessage(BonusAbil) + '/' + EDCode.EncodeMessage(SystemShare.Config.NakedAbilofTaos);
                    break;
            }
            ClientMsg = Messages.MakeMessage(Messages.SM_ADJUST_BONUS, BonusPoint, 0, 0, 0);
            SendSocket(ClientMsg, sSendMsg);
        }

        private void PvpDie(IPlayerActor playObject)
        {
            int nWinLevel = SystemShare.Config.KillHumanWinLevel;
            int nLostLevel = SystemShare.Config.KilledLostLevel;
            int nWinExp = SystemShare.Config.KillHumanWinExp;
            int nLostExp = SystemShare.Config.KillHumanLostExp;
            bool boWinLEvel = SystemShare.Config.IsKillHumanWinLevel;
            bool boLostLevel = SystemShare.Config.IsKilledLostLevel;
            bool boWinExp = SystemShare.Config.IsKillHumanWinExp;
            bool boLostExp = SystemShare.Config.IsKilledLostExp;
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
            if (playObject.Abil.Level - Abil.Level > SystemShare.Config.HumanLevelDiffer)
            {
                if (!IsGoodKilling(this))
                {
                    playObject.IncPkPoint(SystemShare.Config.KillHumanAddPKPoint);
                    playObject.SysMsg(Settings.YouMurderedMsg, MsgColor.Red, MsgType.Hint);
                    SysMsg(Format(Settings.YouKilledByMsg, LastHiter.ChrName), MsgColor.Red, MsgType.Hint);
                    playObject.AddBodyLuck(-SystemShare.Config.KillHumanDecLuckPoint);
                    if (PvpLevel() < 1)
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            playObject.MakeWeaponUnlock();
                        }
                    }
                    if (SystemShare.FunctionNPC != null)
                    {
                        SystemShare.FunctionNPC.GotoLable(playObject, "@OnMurder", false);
                        SystemShare.FunctionNPC.GotoLable(this, "@Murdered", false);
                    }
                }
                else
                {
                    playObject.SysMsg(Settings.YouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                }
                return;
            }
            if (boWinLEvel)
            {
                if (playObject.Abil.Level + nWinLevel <= Settings.MAXUPLEVEL)
                {
                    playObject.Abil.Level += (byte)nWinLevel;
                }
                else
                {
                    playObject.Abil.Level = Settings.MAXUPLEVEL;
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

        private bool CancelGroup()
        {
            bool result = true;
            const string sCanceGrop = "你的小组被解散了.";
            if (GroupMembers.Count <= 1)
            {
                SendGroupText(sCanceGrop);
                GroupMembers.Clear();
                GroupOwner = 0;
                result = false;
            }
            return result;
        }

        private void SendGroupMembers()
        {
            IPlayerActor playObject;
            string sSendMsg = "";
            for (int i = 0; i < GroupMembers.Count; i++)
            {
                playObject = GroupMembers[i];
                sSendMsg = sSendMsg + playObject.ChrName + '/';
            }
            for (int i = 0; i < GroupMembers.Count; i++)
            {
                playObject = GroupMembers[i];
                playObject.SendDefMessage(Messages.SM_GROUPMEMBERS, 0, 0, 0, 0, sSendMsg);
            }
        }

        protected static ushort GetSpellPoint(UserMagic userMagic)
        {
            return (ushort)(HUtil32.Round(userMagic.Magic.Spell / 4.0 * (userMagic.Level + 1)) + userMagic.Magic.DefSpell);
        }

        private bool DoMotaeboCanMotaebo(IActor baseObject, int nMagicLevel)
        {
            if (Abil.Level > baseObject.Abil.Level && !baseObject.StickMode) //当前玩家等级大于目标等级，并且目标可以被冲撞
            {
                int nC = Abil.Level - baseObject.Abil.Level;
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

        protected bool DoMotaebo(byte nDir, byte nMagicLevel)
        {
            int nDmg;
            IActor baseObject34 = null;
            short nX = 0;
            short nY = 0;
            bool result = false;
            bool bo35 = true;
            byte n24 = (byte)(nMagicLevel + 1);
            byte n28 = n24;
            Dir = nDir;
            IActor poseCreate = GetPoseCreate();
            if (poseCreate != null)
            {
                for (int i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
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
                            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref nX, ref nY))
                            {
                                IActor baseObject30 = Envir.GetMovingObject(nX, nY, true);
                                if (baseObject30 != null && DoMotaeboCanMotaebo(baseObject30, nMagicLevel))
                                {
                                    baseObject30.CharPushed(Dir, 1);
                                }
                            }
                        }
                        baseObject34 = poseCreate;
                        if (poseCreate.CharPushed(Dir, 1) != 1)
                        {
                            break;
                        }
                        GetFrontPosition(ref nX, ref nY);
                        if (Envir.MoveToMovingObject(CurrX, CurrY, this, nX, nY, false))
                        {
                            CurrX = nX;
                            CurrY = nY;
                            SendRefMsg(Messages.RM_RUSH, nDir, CurrX, CurrY, 0, "");
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
                for (int i = 0; i < HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    GetFrontPosition(ref nX, ref nY);
                    if (Envir.MoveToMovingObject(CurrX, CurrY, this, nX, nY, false))
                    {
                        CurrX = nX;
                        CurrY = nY;
                        SendRefMsg(Messages.RM_RUSH, nDir, CurrX, CurrY, 0, "");
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
                baseObject34.StruckDamage(nDmg);
                baseObject34.SendRefMsg(Messages.RM_STRUCK, nDmg, baseObject34.WAbil.HP, baseObject34.WAbil.MaxHP, ActorId, "");
                if (baseObject34.Race != ActorRace.Play)
                {
                    baseObject34.SendMsg(baseObject34, Messages.RM_STRUCK, nDmg, baseObject34.WAbil.HP, baseObject34.WAbil.MaxHP, ActorId);
                }
            }
            if (bo35)
            {
                GetFrontPosition(ref nX, ref nY);
                SendRefMsg(Messages.RM_RUSHKUNG, Dir, nX, nY, 0, "");
                SysMsg(Settings.MateDoTooweak, MsgColor.Red, MsgType.Hint);
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
                SendRefMsg(Messages.RM_STRUCK, nDmg, WAbil.HP, WAbil.MaxHP, 0, "");
            }
            return result;
        }

        private bool DoSpell(UserMagic userMagic, short targetX, short targetY, IActor targetObject)
        {
            bool result = false;
            try
            {
                if (!MagicManager.IsWarrSkill(userMagic.MagIdx))
                {
                    ushort nSpellPoint = GetSpellPoint(userMagic);
                    if (nSpellPoint > 0)
                    {
                        if (WAbil.MP < nSpellPoint)
                        {
                            return false;
                        }
                        DamageSpell(nSpellPoint);
                        HealthSpellChanged();
                    }
                    result = MagicManager.DoSpell(this, userMagic, targetX, targetY, targetObject);
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(Format("[Exception] PlayObject.DoSpell MagID:{0} X:{1} Y:{2}", userMagic.MagIdx, targetX, targetY));
                M2Share.Logger.Error(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 挖矿
        /// </summary>
        /// <returns></returns>
        private bool PileStones(int nX, int nY)
        {
            bool result = false;
            string s1C = string.Empty;
            StoneMineEvent mineEvent = (StoneMineEvent)Envir.GetEvent(nX, nY);
            if (mineEvent != null && mineEvent.EventType == Grobal2.ET_MINE)
            {
                if (mineEvent.MineCount > 0)
                {
                    mineEvent.MineCount -= 1;
                    if (M2Share.RandomNumber.Random(SystemShare.Config.MakeMineHitRate) == 0)
                    {
                        PileStones pileEvent = (PileStones)Envir.GetEvent(CurrX, CurrY);
                        if (pileEvent == null)
                        {
                            pileEvent = new PileStones(Envir, CurrX, CurrY, Grobal2.ET_PILESTONES, 5 * 60 * 1000);
                            SystemShare.EventMgr.AddEvent(pileEvent);
                        }
                        else
                        {
                            if (pileEvent.EventType == Grobal2.ET_PILESTONES)
                            {
                                pileEvent.AddEventParam();
                            }
                        }
                        if (M2Share.RandomNumber.Random(SystemShare.Config.MakeMineRate) == 0)
                        {
                            if (Envir.Flag.Mine)
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
            SendRefMsg(Messages.RM_HEAVYHIT, Dir, CurrX, CurrY, 0, s1C);
            return result;
        }

        private void SendSaveItemList(int merchantId)
        {
            string sSendMsg = string.Empty;
            int maxCount = StorageItemList.Count;
            short page = (short)Math.Ceiling(maxCount / 50f);

            for (int p = 0; p < page; p++)
            {
                int startCount = p * 50;
                int endCount = startCount + 50;
                if (endCount > maxCount)
                {
                    endCount = maxCount;
                }
                for (int i = startCount; i < endCount; i++)
                {
                    UserItem userItem = StorageItemList[i];
                    StdItem item = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                    if (item != null)
                    {
                        ClientItem clientItem = new ClientItem();
                        SystemShare.ItemSystem.GetUpgradeStdItem(item, userItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                        clientItem.Dura = userItem.Dura;
                        clientItem.DuraMax = userItem.DuraMax;
                        clientItem.MakeIndex = userItem.MakeIndex;
                        sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + '/';
                    }
                }
                ClientMsg = Messages.MakeMessage(Messages.SM_SAVEITEMLIST, merchantId, 0, p, (page - 1) < 0 ? 0 : page - 1);
                SendSocket(ClientMsg, sSendMsg);
            }
        }

        private void SendChangeGuildName()
        {
            if (MyGuild != null)
            {
                SendDefMessage(Messages.SM_CHANGEGUILDNAME, 0, 0, 0, 0, MyGuild.GuildName + '/' + GuildRankName);
            }
            else
            {
                SendDefMessage(Messages.SM_CHANGEGUILDNAME, 0, 0, 0, 0);
            }
        }

        private void SendDelItemList(IList<DeleteItem> itemList)
        {
            string s10 = string.Empty;
            for (int i = 0; i < itemList.Count; i++)
            {
                s10 = s10 + itemList[i].ItemName + '/' + itemList[i].MakeIndex + '/';
            }
            ClientMsg = Messages.MakeMessage(Messages.SM_DELITEMS, 0, 0, 0, itemList.Count);
            SendSocket(ClientMsg, EDCode.EncodeString(s10));
        }

        public void SendDelItems(UserItem userItem)
        {
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ClientMsg = Messages.MakeMessage(Messages.SM_DELITEM, ActorId, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        public void SendUpdateItem(UserItem userItem)
        {
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ClientMsg = Messages.MakeMessage(Messages.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private void SendUpdateItemWithLevel(UserItem userItem, byte level)
        {
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ChangeItemWithLevel(ref clientItem, level);
                ClientMsg = Messages.MakeMessage(Messages.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private void SendUpdateItemByJob(UserItem userItem, byte level)
        {
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                clientItem.MakeIndex = userItem.MakeIndex;
                clientItem.Dura = userItem.Dura;
                clientItem.DuraMax = userItem.DuraMax;
                if (stdItem.StdMode == 50)
                {
                    clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                }
                ChangeItemByJob(ref clientItem, level);
                ClientMsg = Messages.MakeMessage(Messages.SM_UPDATEITEM, ActorId, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
            }
        }

        private bool CheckTakeOnItems(int nWhere, ref ClientItem clientItem)
        {
            bool result = false;
            switch (clientItem.Item.StdMode)
            {
                case 10 when Gender != PlayGender.Man:
                    SysMsg(Settings.WearNotOfWoMan, MsgColor.Red, MsgType.Hint);
                    return false;
                case 11 when Gender != PlayGender.WoMan:
                    SysMsg(Settings.WearNotOfMan, MsgColor.Red, MsgType.Hint);
                    return false;
            }
            if (nWhere == 1 || nWhere == 2)
            {
                if (clientItem.Item.Weight > WAbil.MaxHandWeight)
                {
                    SysMsg(Settings.HandWeightNot, MsgColor.Red, MsgType.Hint);
                    return false;
                }
            }
            else
            {
                if (clientItem.Item.Weight + GetUserItemWeitht(nWhere) > WAbil.MaxWearWeight)
                {
                    SysMsg(Settings.WearWeightNot, MsgColor.Red, MsgType.Hint);
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
                        SysMsg(Settings.LevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 1:
                    if (HUtil32.HiByte(WAbil.DC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.DCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 10:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && Abil.Level >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.JobOrLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 11:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.DC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.JobOrDCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 12:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.MC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.JobOrMCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 13:
                    if (Job == (PlayJob)HUtil32.LoByte(clientItem.Item.NeedLevel) && HUtil32.HiByte(WAbil.SC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.JobOrSCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 2:
                    if (HUtil32.HiByte(WAbil.MC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.MCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 3:
                    if (HUtil32.HiByte(WAbil.SC) >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.SCNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 4:
                    if (ReLevel >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 40:
                    if (ReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (Abil.Level >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.LevelNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 41:
                    if (ReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.DC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.DCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 42:
                    if (ReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.MC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.MCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 43:
                    if (ReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (HUtil32.HiByte(WAbil.SC) >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.SCNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 44:
                    if (ReLevel >= HUtil32.LoByte(clientItem.Item.NeedLevel))
                    {
                        if (CreditPoint >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.CreditPointNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.ReNewLevelNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 5:
                    if (CreditPoint >= clientItem.Item.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.CreditPointNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 6:
                    if (MyGuild != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.GuildNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 60:
                    if (MyGuild != null && GuildRankNo == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.GuildMasterNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 7:
                    if (MyGuild != null && SystemShare.CastleMgr.IsCastleMember(this) != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.SabukHumanNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 70:
                    if (MyGuild != null && SystemShare.CastleMgr.IsCastleMember(this) != null && GuildRankNo == 1)
                    {
                        if (Abil.Level >= clientItem.Item.NeedLevel)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(Settings.LevelNot, MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        SysMsg(Settings.SabukMasterManNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 8:
                    if (MemberType != 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.MemberNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 81:
                    if (MemberType == HUtil32.LoByte(clientItem.Item.NeedLevel) && MemberLevel >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.MemberTypeNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                case 82:
                    if (MemberType >= HUtil32.LoByte(clientItem.Item.NeedLevel) && MemberLevel >= HUtil32.HiByte(clientItem.Item.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.MemberTypeNot, MsgColor.Red, MsgType.Hint);
                    }
                    break;
            }
            return result;
        }

        private int GetUserItemWeitht(int nWhere)
        {
            int n14 = 0;
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (nWhere == -1 || !(i == nWhere) && !(i == 1) && !(i == 2))
                {
                    if (UseItems[i] == null)
                    {
                        continue;
                    }
                    StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
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
            bool result = false;
            if (Envir.Flag.boNODRUG)
            {
                SysMsg(Settings.CanotUseDrugOnThisMap, MsgColor.Red, MsgType.Hint);
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
                            IncHealthSpell(HUtil32.Round(WAbil.MaxHP / 100.0 * stdItem.AC), HUtil32.Round(WAbil.MaxMP / 100.0 * stdItem.MAC));
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
                    int nOldStatus = GetMyStatus();
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
                            bool boNeedRecalc = false;
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
                                SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
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
            bool result = false;
            MagicInfo magic = SystemShare.WorldEngine.FindMagic(stdItem.Name);
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.MagicId))
                {
                    if (magic.Job == 99 || magic.Job == (byte)Job)
                    {
                        if (Abil.Level >= magic.TrainLevel[0])
                        {
                            UserMagic userMagic = new UserMagic
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
                                SendAddMagic(userMagic);
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
            ClientMagic clientMagic = new ClientMagic
            {
                Key = userMagic.Key,
                Level = userMagic.Level,
                CurTrain = userMagic.TranPoint,
                Def = userMagic.Magic
            };
            ClientMsg = Messages.MakeMessage(Messages.SM_ADDMAGIC, 0, 0, 0, 1);
            SendSocket(ClientMsg, EDCode.EncodeBuffer(clientMagic));
        }

        public void SendDelMagic(UserMagic userMagic)
        {
            ClientMsg = Messages.MakeMessage(Messages.SM_DELMAGIC, userMagic.MagIdx, 0, 0, 1);
            SendSocket(ClientMsg);
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <returns></returns>
        private bool EatUseItems(int nShape)
        {
            bool result = false;
            switch (nShape)
            {
                case 1:
                    SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    BaseObjectMove(HomeMap, 0, 0);
                    result = true;
                    break;
                case 2:
                    if (!Envir.Flag.boNORANDOMMOVE)
                    {
                        SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        BaseObjectMove(MapName, 0, 0);
                        result = true;
                    }
                    break;
                case 3:
                    SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    if (PvpLevel() < 2)
                    {
                        BaseObjectMove(HomeMap, HomeX, HomeY);
                    }
                    else
                    {
                        BaseObjectMove(SystemShare.Config.RedHomeMap, SystemShare.Config.RedHomeX, SystemShare.Config.RedHomeY);
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
                        if (!InGuildWarArea)
                        {
                            IUserCastle castle = SystemShare.CastleMgr.IsCastleMember(this);
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
            SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
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
                short nX = sX;
                short nY = sY;
                SpaceMove(sMap, nX, nY, 0);
            }
            else
            {
                MapRandomMove(sMap, 0);
            }
            IEnvirnoment envir = Envir;
            if (envir != Envir && Race == ActorRace.Play)
            {
                IsTimeRecall = false;
            }
        }

        private void ChangeServerMakeSlave(SlaveInfo slaveInfo)
        {
            int nSlavecount = Job == PlayJob.Taoist ? 1 : 5;
            MonsterObject baseObject = (MonsterObject)MakeSlave(slaveInfo.SlaveName, 3, slaveInfo.SlaveLevel, nSlavecount, slaveInfo.RoyaltySec);
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
                StdItem pStdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (pStdItem != null)
                {
                    ClientItem clientItem = new ClientItem();
                    SystemShare.ItemSystem.GetUpgradeStdItem(pStdItem, userItem, ref clientItem);
                    clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                    clientItem.MakeIndex = userItem.MakeIndex;
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    ClientMsg = Messages.MakeMessage(Messages.SM_DEALREMOTEDELITEM, ActorId, 0, 0, 1);
                    DealCreat.SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
                    DealCreat.DealLastTick = HUtil32.GetTickCount();
                    DealLastTick = HUtil32.GetTickCount();
                }
                SendDefMessage(Messages.SM_DEALDELITEM_OK, 0, 0, 0, 0);
            }
            else
            {
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0);
            }
        }

        private void SendAddDealItem(UserItem userItem)
        {
            SendDefMessage(Messages.SM_DEALADDITEM_OK, 0, 0, 0, 0);
            if (DealCreat != null)
            {
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    ClientItem clientItem = new ClientItem();
                    SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                    clientItem.Item.Name = CustomItemSystem.GetItemName(userItem);
                    clientItem.MakeIndex = userItem.MakeIndex;
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    ClientMsg = Messages.MakeMessage(Messages.SM_DEALREMOTEADDITEM, ActorId, 0, 0, 1);
                    DealCreat.SendSocket(ClientMsg, EDCode.EncodeBuffer(clientItem));
                    DealCreat.DealLastTick = HUtil32.GetTickCount();
                    DealLastTick = HUtil32.GetTickCount();
                }
            }
        }

        public void OpenDealDlg(IPlayerActor baseObject)
        {
            DealCreat = baseObject;
            if (DealCreat == null)
            {
                return;
            }
            Dealing = true;
            GetBackDealItems();
            SendDefMessage(Messages.SM_DEALMENU, 0, 0, 0, 0, DealCreat.ChrName);
            DealLastTick = HUtil32.GetTickCount();
        }

        public void JoinGroup(IPlayerActor playObject)
        {
            GroupOwner = playObject.ActorId;
            SendGroupText(Format(Settings.JoinGroup, ChrName));
        }

        /// <summary>
        /// 随机矿石持久度
        /// </summary>
        /// <returns></returns>
        private static ushort MakeMineRandomDrua()
        {
            int result = M2Share.RandomNumber.Random(SystemShare.Config.StoneGeneralDuraRate) + SystemShare.Config.StoneMinDura;
            if (M2Share.RandomNumber.Random(SystemShare.Config.StoneAddDuraRate) == 0)
            {
                result += M2Share.RandomNumber.Random(SystemShare.Config.StoneAddDuraMax);
            }
            return (ushort)result;
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine()
        {
            UserItem userItem = null;
            if (ItemList.Count >= Grobal2.MaxBagItem)
            {
                return;
            }
            int nRandom = M2Share.RandomNumber.Random(SystemShare.Config.StoneTypeRate);
            if (nRandom >= SystemShare.Config.GoldStoneMin && nRandom <= SystemShare.Config.GoldStoneMax)
            {
                userItem = new UserItem();
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.GoldStone, ref userItem))
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
            if (nRandom >= SystemShare.Config.SilverStoneMin && nRandom <= SystemShare.Config.SilverStoneMax)
            {
                userItem = new UserItem();
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.SilverStone, ref userItem))
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
            if (nRandom >= SystemShare.Config.SteelStoneMin && nRandom <= SystemShare.Config.SteelStoneMax)
            {
                userItem = new UserItem();
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.SteelStone, ref userItem))
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
            if (nRandom >= SystemShare.Config.BlackStoneMin && nRandom <= SystemShare.Config.BlackStoneMax)
            {
                userItem = new UserItem();
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.BlackStone, ref userItem))
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
            if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.CopperStone, ref userItem))
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
            if (ItemList.Count >= Grobal2.MaxBagItem)
            {
                return;
            }
            UserItem mineItem = null;
            int mineRate = M2Share.RandomNumber.Random(120);
            if (HUtil32.RangeInDefined(mineRate, 1, 2))
            {
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.GemStone1, ref mineItem))
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
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.GemStone2, ref mineItem))
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
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.GemStone3, ref mineItem))
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
                if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.GemStone4, ref mineItem))
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
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if (string.Compare(SystemShare.ItemSystem.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
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
            bool result = false;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if (userItem == checkItem)
                {
                    SendDelItems(userItem);
                    Dispose(userItem);
                    ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            for (int i = 0; i < UseItems.Length; i++)
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
            SendMsg(Messages.RM_CHANGEGUILDNAME, 0, 0, 0, 0);
        }

        private bool CheckActionStatus(int wIdent, ref int dwDelayTime)
        {
            bool result = false;
            dwDelayTime = 0;
            if (SystemShare.Config.CloseSpeedHackCheck)
            {
                return true;
            }
            int dwCheckTime;
            if (!SystemShare.Config.DisableStruck) // 检查人物弯腰停留时间
            {
                dwCheckTime = HUtil32.GetTickCount() - StruckTick;
                if (SystemShare.Config.StruckTime > dwCheckTime)
                {
                    dwDelayTime = SystemShare.Config.StruckTime - dwCheckTime;
                    MBtOldDir = Dir;
                    return false;
                }
            }
            // 检查二个不同操作之间所需间隔时间
            dwCheckTime = HUtil32.GetTickCount() - ActionTick;
            if (TestSpeedMode)
            {
                SysMsg("间隔: " + dwCheckTime, MsgColor.Blue, MsgType.Notice);
            }
            if (OldIdent == wIdent)
            {
                // 当二次操作一样时，则将 boFirst 设置为 真 ，退出由调用函数本身检查二个相同操作之间的间隔时间
                return true;
            }
            if (!SystemShare.Config.boControlActionInterval)
            {
                return true;
            }
            int dwActionIntervalTime = ActionIntervalTime;
            switch (wIdent)
            {
                case Messages.CM_LONGHIT:
                    if (SystemShare.Config.boControlRunLongHit && OldIdent == Messages.CM_RUN && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Messages.CM_HIT:
                    if (SystemShare.Config.boControlWalkHit && OldIdent == Messages.CM_WALK && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = WalkHitIntervalTime; // 走位攻击
                    }
                    if (SystemShare.Config.boControlRunHit && OldIdent == Messages.CM_RUN && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunHitIntervalTime;// 跑位攻击
                    }
                    break;
                case Messages.CM_RUN:
                    if (SystemShare.Config.boControlRunLongHit && OldIdent == Messages.CM_LONGHIT && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunLongHitIntervalTime;// 跑位刺杀
                    }
                    if (SystemShare.Config.boControlRunHit && OldIdent == Messages.CM_HIT && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunHitIntervalTime;// 跑位攻击
                    }
                    if (SystemShare.Config.boControlRunMagic && OldIdent == Messages.CM_SPELL && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunMagicIntervalTime;// 跑位魔法
                    }
                    break;
                case Messages.CM_WALK:
                    if (SystemShare.Config.boControlWalkHit && OldIdent == Messages.CM_HIT && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = WalkHitIntervalTime;// 走位攻击
                    }
                    if (SystemShare.Config.boControlRunLongHit && OldIdent == Messages.CM_LONGHIT && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case Messages.CM_SPELL:
                    if (SystemShare.Config.boControlRunMagic && OldIdent == Messages.CM_RUN && MBtOldDir != Dir)
                    {
                        dwActionIntervalTime = RunMagicIntervalTime;// 跑位魔法
                    }
                    break;
            }
            // 将几个攻击操作合并成一个攻击操作代码
            if (wIdent == Messages.CM_HIT || wIdent == Messages.CM_HEAVYHIT || wIdent == Messages.CM_BIGHIT || wIdent == Messages.CM_POWERHIT || wIdent == Messages.CM_WIDEHIT || wIdent == Messages.CM_FIREHIT)
            {
                wIdent = Messages.CM_HIT;
            }
            if (dwCheckTime >= dwActionIntervalTime)
            {
                ActionTick = HUtil32.GetTickCount();
                result = true;
            }
            else
            {
                dwDelayTime = dwActionIntervalTime - dwCheckTime;
            }
            OldIdent = wIdent;
            MBtOldDir = Dir;
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
            string sText = string.Empty;
            CanJmpScriptLableMap.Clear();
            while (true)
            {
                if (string.IsNullOrEmpty(sMsg))
                {
                    break;
                }
                sMsg = HUtil32.GetValidStr3(sMsg, ref sText, '\\');
                if (string.IsNullOrEmpty(sText)) continue;
                MatchCollection matches = M2Share.MatchScriptLabel(sText);
                if (matches.Count <= 0) continue;
                foreach (Match item in matches)
                {
                    string sCmdStr = item.Value;
                    string sLabel = HUtil32.GetValidStr3(sCmdStr, ref sCmdStr, HUtil32.Backslash);
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
            if (string.Compare(sLabel, PlayDiceLabel, StringComparison.OrdinalIgnoreCase) == 0)
            {
                PlayDiceLabel = string.Empty;
                return true;
            }
            return false;
        }

        private bool CheckItemsNeed(StdItem stdItem)
        {
            bool result = true;
            IUserCastle castle = SystemShare.CastleMgr.IsCastleMember(this);
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
                    if (MemberType == 0)
                    {
                        result = false;
                    }
                    break;
                case 81:
                    if (MemberType != HUtil32.LoWord(stdItem.NeedLevel) || MemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
                case 82:
                    if (MemberType < HUtil32.LoWord(stdItem.NeedLevel) || MemberLevel < HUtil32.HiWord(stdItem.NeedLevel))
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
            bool boIsfound = false;
            string sUnMarryFileName = M2Share.GetEnvirFilePath("UnMarry.txt");
            if (File.Exists(sUnMarryFileName))
            {
                using StringList loadList = new StringList();
                loadList.LoadFromFile(sUnMarryFileName);
                for (int i = 0; i < loadList.Count; i++)
                {
                    if (string.Compare(loadList[i], ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        loadList.RemoveAt(i);
                        boIsfound = true;
                        break;
                    }
                }
                loadList.SaveToFile(sUnMarryFileName);
            }
            if (boIsfound)
            {
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(Settings.fUnMarryManLoginMsg, DearName, DearName);
                }
                else
                {
                    sSayMsg = string.Format(Settings.fUnMarryWoManLoginMsg, ChrName, ChrName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                DearName = "";
                RefShowName();
            }
            DearHuman = SystemShare.WorldEngine.GetPlayObject(DearName);
            if (DearHuman != null)
            {
                DearHuman.DearHuman = this;
                if (Gender == PlayGender.Man)
                {
                    sSayMsg = string.Format(Settings.ManLoginDearOnlineSelfMsg, DearName, ChrName, DearHuman.Envir.MapDesc, DearHuman.CurrX, DearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(Settings.ManLoginDearOnlineDearMsg, DearName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    DearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    sSayMsg = string.Format(Settings.WoManLoginDearOnlineSelfMsg, DearName, ChrName, DearHuman.Envir.MapDesc, DearHuman.CurrX, DearHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(Settings.WoManLoginDearOnlineDearMsg, DearName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    DearHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
            }
            else
            {
                if (Gender == PlayGender.Man)
                {
                    SysMsg(Settings.ManLoginDearNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    SysMsg(Settings.WoManLoginDearNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private void CheckMaster()
        {
            bool boIsfound = false;
            string sSayMsg;
            for (int i = 0; i < M2Share.UnForceMasterList.Count; i++) // 处理强行脱离师徒关系
            {
                if (string.Compare(M2Share.UnForceMasterList[i], ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.UnForceMasterList.RemoveAt(i);
                    M2Share.SaveUnForceMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound)
            {
                if (IsMaster)
                {
                    sSayMsg = string.Format(Settings.fUnMasterLoginMsg, MasterName);
                }
                else
                {
                    sSayMsg = string.Format(Settings.fUnMasterListLoginMsg, MasterName);
                }
                SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                MasterName = "";
                RefShowName();
            }
            if (!string.IsNullOrEmpty(MasterName) && !IsMaster)
            {
                if (Abil.Level >= SystemShare.Config.MasterOKLevel)
                {
                    IPlayerActor human = SystemShare.WorldEngine.GetPlayObject(MasterName);
                    if (human != null && !human.Death && !human.Ghost)
                    {
                        sSayMsg = string.Format(Settings.YourMasterListUnMasterOKMsg, ChrName);
                        human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        SysMsg(Settings.YouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        if (ChrName == human.MasterName)// 如果大徒弟则将师父上的名字去掉
                        {
                            human.MasterName = "";
                            human.RefShowName();
                        }
                        for (int i = 0; i < human.MasterList.Count; i++)
                        {
                            if (human.MasterList[i] == this)
                            {
                                human.MasterList.RemoveAt(i);
                                break;
                            }
                        }
                        MasterName = "";
                        RefShowName();
                        if (human.CreditPoint + SystemShare.Config.MasterOKCreditPoint <= byte.MaxValue)
                        {
                            human.CreditPoint += (byte)SystemShare.Config.MasterOKCreditPoint;
                        }
                        human.BonusPoint += SystemShare.Config.nMasterOKBonusPoint;
                        human.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                    }
                    else
                    {
                        // 如果师父不在线则保存到记录表中
                        boIsfound = false;
                        for (int i = 0; i < M2Share.UnMasterList.Count; i++)
                        {
                            if (string.Compare(M2Share.UnMasterList[i], ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                boIsfound = true;
                                break;
                            }
                        }
                        if (!boIsfound)
                        {
                            M2Share.UnMasterList.Add(MasterName);
                        }
                        if (!boIsfound)
                        {
                            M2Share.SaveUnMasterList();
                        }
                        SysMsg(Settings.YouAreUnMasterOKMsg, MsgColor.Red, MsgType.Hint);
                        MasterName = "";
                        RefShowName();
                    }
                }
            }
            // 处理出师记录
            boIsfound = false;
            for (int i = 0; i < M2Share.UnMasterList.Count; i++)
            {
                if (string.Compare(M2Share.UnMasterList[i], ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    M2Share.UnMasterList.RemoveAt(i);
                    M2Share.SaveUnMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound && IsMaster)
            {
                SysMsg(Settings.UnMasterLoginMsg, MsgColor.Red, MsgType.Hint);
                MasterName = "";
                RefShowName();
                if (CreditPoint + SystemShare.Config.MasterOKCreditPoint <= byte.MaxValue)
                {
                    CreditPoint += (byte)SystemShare.Config.MasterOKCreditPoint;
                }
                BonusPoint += SystemShare.Config.nMasterOKBonusPoint;
                SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
            }
            if (string.IsNullOrEmpty(MasterName))
            {
                return;
            }
            if (IsMaster) // 师父上线通知
            {
                MasterHuman = SystemShare.WorldEngine.GetPlayObject(MasterName);
                if (MasterHuman != null)
                {
                    MasterHuman.MasterHuman = this;
                    MasterList.Add(MasterHuman);
                    sSayMsg = string.Format(Settings.MasterOnlineSelfMsg, MasterName, ChrName, MasterHuman.Envir.MapDesc, MasterHuman.CurrX, MasterHuman.CurrY);
                    SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    sSayMsg = string.Format(Settings.MasterOnlineMasterListMsg, MasterName, ChrName, Envir.MapDesc, CurrX, CurrY);
                    MasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                }
                else
                {
                    SysMsg(Settings.MasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                // 徒弟上线通知
                if (!string.IsNullOrEmpty(MasterName))
                {
                    MasterHuman = SystemShare.WorldEngine.GetPlayObject(MasterName);
                    if (MasterHuman != null)
                    {
                        if (MasterHuman.MasterName == ChrName)
                        {
                            MasterHuman.MasterHuman = this;
                        }
                        MasterHuman.MasterList.Add(this);
                        sSayMsg = string.Format(Settings.MasterListOnlineSelfMsg, MasterName, ChrName, MasterHuman.Envir.MapDesc, MasterHuman.CurrX, MasterHuman.CurrY);
                        SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                        sSayMsg = string.Format(Settings.MasterListOnlineMasterMsg, MasterName, ChrName, Envir.MapDesc, CurrX, CurrY);
                        MasterHuman.SysMsg(sSayMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        SysMsg(Settings.MasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    }
                }
            }
        }

        public string GetMyInfo()
        {
            string sMyInfo = Settings.MyInfo;
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
            sMyInfo = sMyInfo.Replace("%logontime", DateTimeOffset.FromUnixTimeMilliseconds(LogonTime).ToString("yyyy-MM-dd HH:ss:mm"));
            sMyInfo = sMyInfo.Replace("%logonint", ((HUtil32.GetTickCount() - LogonTick) / 60000).ToString());
            return sMyInfo;
        }

        public bool CheckItemBindUse(UserItem userItem)
        {
            ItemBind itemBind;
            bool result = true;
            for (int i = 0; i < M2Share.ItemBindAccount.Count; i++)
            {
                itemBind = M2Share.ItemBindAccount[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, UserAccount, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.ItemIsNotThisAccount, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            for (int i = 0; i < M2Share.ItemBindIPaddr.Count; i++)
            {
                itemBind = M2Share.ItemBindIPaddr[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, LoginIpAddr, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.ItemIsNotThisIPaddr, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            for (int i = 0; i < M2Share.ItemBindChrName.Count; i++)
            {
                itemBind = M2Share.ItemBindChrName[i];
                if (itemBind.nMakeIdex == userItem.MakeIndex && itemBind.nItemIdx == userItem.Index)
                {
                    result = false;
                    if (string.Compare(itemBind.sBindName, ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(Settings.ItemIsNotThisChrName, MsgColor.Red, MsgType.Hint);
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
                //ProcessUserLineMsg("@" + CommandMgr.GameCommands.Unlock.CmdName);
                return;
            }
            string sData = processMsg.Msg;
            int nLen = sData.Length;
            if (IsSetStoragePwd)
            {
                IsSetStoragePwd = false;
                if (nLen > 3 && nLen < 8)
                {
                    MSTempPwd = sData;
                    IsReConfigPwd = true;
                    SysMsg(Settings.ReSetPasswordMsg, MsgColor.Green, MsgType.Hint);// '请重复输入一次仓库密码：'
                    SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                }
                else
                {
                    SysMsg(Settings.PasswordOverLongMsg, MsgColor.Red, MsgType.Hint);// '输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                }
                return;
            }
            if (IsReConfigPwd)
            {
                IsReConfigPwd = false;
                if (string.Compare(MSTempPwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    StoragePwd = sData;
                    IsPasswordLocked = true;
                    SysMsg(Settings.ReSetPasswordOKMsg, MsgColor.Blue, MsgType.Hint);// '密码设置成功!!，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。'
                }
                else
                {
                    SysMsg(Settings.ReSetPasswordNotMatchMsg, MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (IsUnLockPwd || IsUnLockStoragePwd)
            {
                if (string.Compare(StoragePwd, sData, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    IsPasswordLocked = false;
                    if (IsUnLockPwd)
                    {
                        if (SystemShare.Config.LockDealAction)
                        {
                            IsCanDeal = true;
                        }
                        if (SystemShare.Config.LockDropAction)
                        {
                            IsCanDrop = true;
                        }
                        if (SystemShare.Config.LockWalkAction)
                        {
                            IsCanWalk = true;
                        }
                        if (SystemShare.Config.LockRunAction)
                        {
                            IsCanRun = true;
                        }
                        if (SystemShare.Config.LockHitAction)
                        {
                            IsCanHit = true;
                        }
                        if (SystemShare.Config.LockSpellAction)
                        {
                            IsCanSpell = true;
                        }
                        if (SystemShare.Config.LockSendMsgAction)
                        {
                            IsCanSendMsg = true;
                        }
                        if (SystemShare.Config.LockUserItemAction)
                        {
                            BoCanUseItem = true;
                        }
                        if (SystemShare.Config.LockInObModeAction)
                        {
                            ObMode = false;
                            AdminMode = false;
                        }
                        IsLockLogoned = true;
                        SysMsg(Settings.PasswordUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                    if (IsUnLockStoragePwd)
                    {
                        if (SystemShare.Config.LockGetBackItemAction)
                        {
                            IsCanGetBackItem = true;
                        }
                        SysMsg(Settings.StorageUnLockOKMsg, MsgColor.Blue, MsgType.Hint);
                    }
                }
                else
                {
                    PwdFailCount++;
                    SysMsg(Settings.UnLockPasswordFailMsg, MsgColor.Red, MsgType.Hint);
                    if (PwdFailCount > 3)
                    {
                        SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                    }
                }
                IsUnLockPwd = false;
                IsUnLockStoragePwd = false;
                return;
            }
            if (IsCheckOldPwd)
            {
                IsCheckOldPwd = false;
                if (StoragePwd == sData)
                {
                    SendMsg(Messages.RM_PASSWORD, 0, 0, 0, 0);
                    SysMsg(Settings.SetPasswordMsg, MsgColor.Green, MsgType.Hint);
                    IsSetStoragePwd = true;
                }
                else
                {
                    PwdFailCount++;
                    SysMsg(Settings.OldPasswordIncorrectMsg, MsgColor.Red, MsgType.Hint);
                    if (PwdFailCount > 3)
                    {
                        SysMsg(Settings.StoragePasswordLockedMsg, MsgColor.Red, MsgType.Hint);
                        IsPasswordLocked = true;
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
            IPlayerActor playObject = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (playObject != null)
            {
                if (GetFrontPosition(ref nX, ref nY))
                {
                    if (GetRecallXy(nX, nY, 3, ref n18, ref n1C))
                    {
                        playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
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
                // SysMsg(Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
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
            IGuild guild = SystemShare.GuildMgr.FindGuild(sGuildName);
            if (guild == null)
            {
                SysMsg("行会不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boReQuestOk = false;
            WarGuild warGuild = MyGuild.AddWarGuild(guild);
            if (warGuild.WarTick > 0)
            {
                if (guild.AddWarGuild(MyGuild).Guild != null)
                {
                    warGuild.WarTick = 0;
                }
                else
                {
                    boReQuestOk = true;
                }
            }
            if (boReQuestOk)
            {
                SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, guild.GuildName);
            }
        }

        private bool CheckDenyLogon()
        {
            bool result = false;
            if (M2Share.GetDenyIPAddrList(LoginIpAddr))
            {
                SysMsg(Settings.YourIPaddrDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyAccountList(UserAccount))
            {
                SysMsg(Settings.YourAccountDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            else if (M2Share.GetDenyChrNameList(ChrName))
            {
                SysMsg(Settings.YourChrNameDenyLogon, MsgColor.Red, MsgType.Hint);
                result = true;
            }
            if (result)
            {
                BoEmergencyClose = true;
            }
            return result;
        }

        /// <summary>
        /// 转移到指定位面服务器
        /// </summary>
        public void TransferPlanesServer(string serveraddr, int gamePort)
        {
            SendMsg(Messages.RM_RECONNECTION, 0, 0, 0, 0, serveraddr + '/' + gamePort);
        }

        private void ProcessQueryValue(int npc, string sData)
        {
            INormNpc normNpc;
            string sRefMsg = string.Empty;
            if (!Ghost && !string.IsNullOrEmpty(GotoNpcLabel))
            {
                sRefMsg = EDCode.DeCodeString(sData);
                //if (IsInGuildRankNameFilterList(sRefMsg))
                //{
                //    SendMsg(M2Share.g_ManageNPC, Messages.RM_MENU_OK, 0, this.ObjectId, 0, 0, sIsInQVFilterListMsg);
                //    return;
                //}
            }
            switch (ValType)
            {
                case 0:
                    MNSval[ValLabel] = sRefMsg;
                    break;
                case 1:
                    MNMval[ValLabel] = HUtil32.StrToInt(sRefMsg, 0);
                    break;
            }
            switch (ValNpcType)
            {
                case 0:
                    normNpc = SystemShare.WorldEngine.FindMerchant(npc);
                    if (normNpc == null)
                    {
                        normNpc = SystemShare.WorldEngine.FindNpc(npc);
                    }
                    if (normNpc != null)
                    {
                        if (normNpc.Envir == Envir && Math.Abs(normNpc.CurrX - CurrX) <= 15 && Math.Abs(normNpc.CurrY - CurrY) <= 15)
                        {
                            normNpc.GotoLable(this, GotoNpcLabel, false);
                        }
                    }
                    break;
                case 1:
                    if (SystemShare.FunctionNPC != null)
                    {
                        SystemShare.FunctionNPC.GotoLable(this, GotoNpcLabel, false);
                    }
                    break;
                case 2:
                    if (SystemShare.ManageNPC != null)
                    {
                        SystemShare.ManageNPC.GotoLable(this, GotoNpcLabel, false);
                    }
                    break;
            }
            GotoNpcLabel = string.Empty;
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
                INormNpc npc = SystemShare.WorldEngine.FindMerchant(nParam1) ?? SystemShare.WorldEngine.FindNpc(nParam1);
                if (npc != null)
                {
                    LastNpc = npc.ActorId;
                }
                if (npc == null)
                {
                    return;
                }
                if (npc.Envir == Envir && IsWithinSight(npc) || npc.IsHide)
                {
                    DlgItemIndex = HUtil32.MakeLong(nParam2, nParam3);
                    int nTemp;
                    if (TakeDlgItem && DlgItemIndex > 0)
                    {
                        nTemp = 255;
                        for (int i = 0; i < ItemList.Count; i++)
                        {
                            UserItem userItem = ItemList[i];
                            if (userItem.Index == DlgItemIndex)
                            {
                                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                                if (stdItem != null)
                                {
                                    if (stdItem.NeedIdentify == 1)
                                    {
                                        // M2Share.ItemEventSource.AddGameLog('10' + #9 + MapName + #9 +inttostr(CurrX) + #9 + inttostr(CurrY) + #9 +m_sChrName + #9 + StdItem.Name + #9 +inttostr(UserItem.MakeIndex) + #9 + '1' + #9 + m_sChrName);
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
                    SendDefMessage(Messages.SM_ITEMDLGSELECT, 1, nTemp, 0, 0);
                    npc.GotoLable(this, GotoNpcLabel);
                    GotoNpcLabel = string.Empty;
                }
            }
        }

        private int GetGuildRelation(IPlayerActor play, IPlayerActor target)
        {
            GuildWarArea = false;
            if ((play.MyGuild == null) || (target.MyGuild == null))
            {
                return 0;
            }
            if (play.InSafeArea() || target.InSafeArea())
            {
                return 0;
            }
            if (play.MyGuild.GuildWarList.Count <= 0)
            {
                return 0;
            }
            GuildWarArea = true;
            int result = 0;
            if (play.MyGuild.IsWarGuild(target.MyGuild) && target.MyGuild.IsWarGuild(play.MyGuild))
            {
                result = 2;
            }
            if (play.MyGuild == target.MyGuild)
            {
                result = 1;
            }
            if (play.MyGuild.IsAllyGuild(target.MyGuild) && target.MyGuild.IsAllyGuild(play.MyGuild))
            {
                result = 3;
            }
            return result;
        }
    }
}