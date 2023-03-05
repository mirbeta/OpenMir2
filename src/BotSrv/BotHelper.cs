using System;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace BotSrv
{
    public static class BotHelper
    {
        private static readonly IList<ClientItem> DropItems = null;

        public static int GetCodeMsgSize(float len)
        {
            if ((int)len < len)
            {
                return (int)(Math.Truncate(len) + 1);
            }
            return (int)Math.Truncate(len);
        }
        
        public static void GetNextHitPosition(short sX, short sY, ref short newX, ref short newY)
        {
            var dir = GetNextDirection(sX, sY, newX, newY);
            newX = sX;
            newY = sY;
            switch (dir)
            {
                case Direction.Up:
                    newY = (short)(newY - 2);
                    break;
                case Direction.Down:
                    newY = (short)(newY + 2);
                    break;
                case Direction.Left:
                    newX = (short)(newX + 2);
                    break;
                case Direction.Right:
                    newX = (short)(newX - 2);
                    break;
                case Direction.UpLeft:
                    newX = (short)(newX - 2);
                    newY = (short)(newY - 2);
                    break;
                case Direction.UpRight:
                    newX = (short)(newX + 2);
                    newY = (short)(newY + 2);
                    break;
                case Direction.DownLeft:
                    newX = (short)(newX - 2);
                    newY = (short)(newY + 2);
                    break;
                case Direction.DownRight:
                    newX = (short)(newX + 2);
                    newY = (short)(newY + 2);
                    break;
            }
        }

        public static void ClearBag()
        {
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                MShare.ItemArr[i].Item.Name = "";
            }
        }

        public static void AddItemBag(ClientItem cu, int idx = 0)
        {
            bool inputCheck = false;
            if (cu == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(cu.Item.Name))
            {
                return;
            }
            if (idx >= 0)
            {
                if (MShare.ItemArr[idx] == null || MShare.ItemArr[idx].Item.Name == "")
                {
                    MShare.ItemArr[idx] = cu;
                    return;
                }
            }
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.ItemArr[i].MakeIndex == cu.MakeIndex) && (MShare.ItemArr[i].Item.Name == cu.Item.Name))
                {
                    return;
                }
            }
            if (string.IsNullOrEmpty(cu.Item.Name))
            {
                return;
            }
            if (cu.Item.StdMode <= 3)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (MShare.ItemArr[i] == null || MShare.ItemArr[i].Item.Name == "")
                    {
                        MShare.ItemArr[i] = cu;
                        return;
                    }
                }
            }
            for (var i = 6; i < BotConst.MaxBagItemcl; i++)
            {
                if ((MShare.ItemArr[i].Item.Name == cu.Item.Name) && (MShare.ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.ItemArr[i].Dura = (ushort)(MShare.ItemArr[i].Dura + cu.Dura);
                    cu.Dura = 0;
                    inputCheck = true;
                }
            }
            if (!inputCheck)
            {
                for (var i = 6; i < BotConst.MaxBagItemcl; i++)
                {
                    if (string.IsNullOrEmpty(MShare.ItemArr[i].Item.Name))
                    {
                        MShare.ItemArr[i] = cu;
                        break;
                    }
                }
            }
            ArrangeItembag();
        }

        public static void UpdateItemBag(ClientItem cu)
        {
            for (var i = BotConst.MaxBagItemcl - 1; i >= 0; i--)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.ItemArr[i].Item.Name == cu.Item.Name) && (MShare.ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.ItemArr[i] = cu;
                    break;
                }
            }
        }

        public static void UpdateBagStallItem(ClientItem cu, byte ststus)
        {
            for (var i = BotConst.MaxBagItemcl - 1; i >= 6; i--)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.ItemArr[i].Item.Name == cu.Item.Name) && (MShare.ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.ItemArr[i].Item.NeedIdentify = ststus;
                    break;
                }
            }
        }

        public static bool FillBagStallItem(byte ststus)
        {
            bool result = false;
            for (var i = BotConst.MaxBagItemcl - 1; i >= 6; i--)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.ItemArr[i].Item.Name != "") && (MShare.ItemArr[i].Item.NeedIdentify != ststus))
                {
                    MShare.ItemArr[i].Item.NeedIdentify = ststus;
                    result = true;
                }
            }
            return result;
        }

        public static void DelItemBag(string iname, int iindex)
        {
            for (var i = BotConst.MaxBagItemcl - 1; i >= 0; i--)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.ItemArr[i].Item.Name == iname) && (MShare.ItemArr[i].MakeIndex == iindex))
                {
                    break;
                }
            }
            if (MShare.MySelf != null)
            {
                for (var i = 10 - 1; i >= 0; i--)
                {
                    if ((MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name == iname) && (MShare.MySelf.StallMgr.mBlock.Items[i].MakeIndex == iindex))
                    {
                        break;
                    }
                }
            }
            ArrangeItembag();
        }

        public static void ArrangeItembag()
        {
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if (MShare.ItemArr[i].Item.Name != "")
                {
                    for (var k = i + 1; k < BotConst.MaxBagItemcl; k++)
                    {
                        if (MShare.ItemArr[k] == null)
                        {
                            continue;
                        }
                        if ((MShare.ItemArr[i].Item.Name == MShare.ItemArr[k].Item.Name) && (MShare.ItemArr[i].MakeIndex == MShare.ItemArr[k].MakeIndex))
                        {
                            //if (MShare.g_ItemArr[i].Item.Overlap > 0)
                            //{
                            //    MShare.g_ItemArr[i].Dura = MShare.g_ItemArr[i].Dura + MShare.g_ItemArr[k].Dura;
                            //}
                        }
                    }
                    if (MShare.MovingItem != null)
                    {
                        if ((MShare.ItemArr[i].Item.Name == MShare.MovingItem.Item.Item.Name) && (MShare.ItemArr[i].MakeIndex == MShare.MovingItem.Item.MakeIndex))
                        {
                            MShare.MovingItem.Index = 0;
                            MShare.MovingItem.Item.Item.Name = "";
                            MShare.g_boItemMoving = false;
                        }
                    }
                }
            }
            for (var i = 46; i < BotConst.MaxBagItemcl; i++)
            {
                if (MShare.ItemArr[i] == null)
                {
                    continue;
                }
                if (MShare.ItemArr[i].Item.Name != "")
                {
                    for (var k = 6; k < 45; k++)
                    {
                        if (MShare.ItemArr[k].Item.Name == "")
                        {
                            MShare.ItemArr[k] = MShare.ItemArr[i];
                            MShare.ItemArr[i].Item.Name = "";
                            break;
                        }
                    }
                }
            }
        }

        public static void AddDropItem(ClientItem ci)
        {
            DropItems.Add(ci);
        }

        public static ClientItem GetDropItem(string iname, int makeIndex)
        {
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == makeIndex))
                {
                    return DropItems[i];
                }
            }
            return null;
        }

        public static void DelDropItem(string iname, int makeIndex)
        {
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == makeIndex))
                {
                    DropItems[i] = null;
                    DropItems.RemoveAt(i);
                    break;
                }
            }
        }

        public static ClientItem GetDrosItem(string iname, int makeIndex)
        {
            ClientItem result = null;
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == makeIndex))
                {
                    result = DropItems[i];
                    break;
                }
            }
            return result;
        }

        public static void DelDroItemItem(string iname, int makeIndex)
        {
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == makeIndex))
                {
                    DropItems[i] = null;
                    DropItems.RemoveAt(i);
                    break;
                }
            }
        }

        public static void AddDealItem(ClientItem ci)
        {
            for (var i = 0; i < 10; i++)
            {
                if ((MShare.g_DealItems[i].Item.Name == ci.Item.Name))
                {
                    MShare.g_DealItems[i].Dura = (ushort)(MShare.g_DealItems[i].Dura + ci.Dura);
                    break;
                }
                else if (MShare.g_DealItems[i].Item.Name == "")
                {
                    MShare.g_DealItems[i] = ci;
                    break;
                }
            }
        }

        public static void AddYbDealItem(ClientItem ci)
        {
            for (var i = 0; i < 8; i++)
            {
                if (MShare.g_YbDealItems[i].Item.Name == "")
                {
                    MShare.g_YbDealItems[i] = ci;
                    break;
                }
            }
        }

        public static bool CanAddStallItem()
        {
            bool result = false;
            for (var i = 0; i < 10; i++)
            {
                if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name == "")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static int StallItemCount()
        {
            int result = 0;
            for (var i = 0; i < 10; i++)
            {
                if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name != "")
                {
                    result++;
                }
            }
            return result;
        }

        public static bool AddStallItem(ClientItem ci)
        {
            bool result = false;
            for (var i = 0; i < 10; i++)
            {
                if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name != "")
                {
                    if (MShare.MySelf.StallMgr.mBlock.Items[i].MakeIndex == ci.MakeIndex)
                    {
                        MShare.MySelf.StallMgr.mBlock.Items[i] = ci;
                        return true;
                    }
                }
            }
            for (var i = 0; i < 10; i++)
            {
                if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name == "")
                {
                    MShare.MySelf.StallMgr.mBlock.Items[i] = ci;
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static bool DelStallItem(ClientItem ci)
        {
            bool result = false;
            for (var i = 0; i < 10; i++)
            {
                if ((MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name == ci.Item.Name) && (ci.MakeIndex == MShare.MySelf.StallMgr.mBlock.Items[i].MakeIndex))
                {
                    MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name = "";
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static void DelDealItem(ClientItem ci)
        {
            for (var i = 0; i < 10; i++)
            {
                if ((MShare.g_DealItems[i].Item.Name == ci.Item.Name) && (MShare.g_DealItems[i].MakeIndex == ci.MakeIndex))
                {
                    break;
                }
            }
        }

        public static void MoveDealItemToBag()
        {
            for (var i = 0; i < 10; i++)
            {
                if (MShare.g_DealItems[i].Item.Name != "")
                {
                    AddItemBag(MShare.g_DealItems[i]);
                }
            }
        }

        public static void AddDealRemoteItem(ClientItem ci)
        {
            for (var i = 0; i < 20; i++)
            {
                if ((MShare.g_DealRemoteItems[i].Item.Name == ci.Item.Name))
                {
                    MShare.g_DealRemoteItems[i].MakeIndex = ci.MakeIndex;
                }
                else if (MShare.g_DealRemoteItems[i].Item.Name == "")
                {
                    MShare.g_DealRemoteItems[i] = ci;
                    break;
                }
            }
        }

        public static void DelDealRemoteItem(ClientItem ci)
        {
            for (var i = 0; i < 20; i++)
            {
                if ((MShare.g_DealRemoteItems[i].Item.Name == ci.Item.Name) && (MShare.g_DealRemoteItems[i].MakeIndex == ci.MakeIndex))
                {
                    //FillChar(MShare.g_DealRemoteItems[i], '\0');
                    break;
                }
            }
        }

        public static int GetDistance(int sX, int sY, int dx, int dy)
        {
            return HUtil32._MAX(Math.Abs(sX - dx), Math.Abs(sY - dy));
        }

        public static void GetNextPosXy(byte dir, ref short x, ref short y)
        {
            switch (dir)
            {
                case Direction.Up:
                    y = (short)(y - 1);
                    break;
                case Direction.UpRight:
                    x = (short)(x + 1);
                    y = (short)(y - 1);
                    break;
                case Direction.Right:
                    x = (short)(x + 1);
                    break;
                case Direction.DownRight:
                    x = (short)(x + 1);
                    y = (short)(y + 1);
                    break;
                case Direction.Down:
                    y = (short)(y + 1);
                    break;
                case Direction.DownLeft:
                    x = (short)(x - 1);
                    y = (short)(y + 1);
                    break;
                case Direction.Left:
                    x = (short)(x - 1);
                    break;
                case Direction.UpLeft:
                    x = (short)(x - 1);
                    y = (short)(y - 1);
                    break;
            }
        }

        public static void GetNextRunXy(int dir, ref short x, ref short y)
        {
            switch (dir)
            {
                case Direction.Up:
                    y = (short)(y - 2);
                    break;
                case Direction.UpRight:
                    x = (short)(x + 2);
                    y = (short)(y - 2);
                    break;
                case Direction.Right:
                    x = (short)(x + 2);
                    break;
                case Direction.DownRight:
                    x = (short)(x + 2);
                    y = (short)(y + 2);
                    break;
                case Direction.Down:
                    y = (short)(y + 2);
                    break;
                case Direction.DownLeft:
                    x = (short)(x - 2);
                    y = (short)(y + 2);
                    break;
                case Direction.Left:
                    x = (short)(x - 2);
                    break;
                case Direction.UpLeft:
                    x = (short)(x - 2);
                    y = (short)(y - 2);
                    break;
            }
        }

        public static void GetNextHorseRunXy(byte dir, ref short x, ref short y)
        {
            switch (dir)
            {
                case Direction.Up:
                    y = (short)(y - 3);
                    break;
                case Direction.UpRight:
                    x = (short)(x + 3);
                    y = (short)(y - 3);
                    break;
                case Direction.Right:
                    x = (short)(x + 3);
                    break;
                case Direction.DownRight:
                    x = (short)(x + 3);
                    y = (short)(y + 3);
                    break;
                case Direction.Down:
                    y = (short)(y + 3);
                    break;
                case Direction.DownLeft:
                    x = (short)(x - 3);
                    y = (short)(y + 3);
                    break;
                case Direction.Left:
                    x = (short)(x - 3);
                    break;
                case Direction.UpLeft:
                    x = (short)(x - 3);
                    y = (short)(y - 3);
                    break;
            }
        }

        public static byte GetNextDirection(int sX, int sY, int dx, int dy)
        {
            int flagx;
            int flagy;
            byte result = Direction.Down;
            if (sX < dx)
            {
                flagx = 1;
            }
            else if (sX == dx)
            {
                flagx = 0;
            }
            else
            {
                flagx = -1;
            }
            if (Math.Abs(sY - dy) > 2)
            {
                if ((sX >= dx - 1) && (sX <= dx + 1))
                {
                    flagx = 0;
                }
            }
            if (sY < dy)
            {
                flagy = 1;
            }
            else if (sY == dy)
            {
                flagy = 0;
            }
            else
            {
                flagy = -1;
            }
            if (Math.Abs(sX - dx) > 2)
            {
                if ((sY > dy - 1) && (sY <= dy + 1))
                {
                    flagy = 0;
                }
            }
            if ((flagx == 0) && (flagy == -1))
            {
                result = Direction.Up;
            }
            if ((flagx == 1) && (flagy == -1))
            {
                result = Direction.UpRight;
            }
            if ((flagx == 1) && (flagy == 0))
            {
                result = Direction.Right;
            }
            if ((flagx == 1) && (flagy == 1))
            {
                result = Direction.DownRight;
            }
            if ((flagx == 0) && (flagy == 1))
            {
                result = Direction.Down;
            }
            if ((flagx == -1) && (flagy == 1))
            {
                result = Direction.DownLeft;
            }
            if ((flagx == -1) && (flagy == 0))
            {
                result = Direction.Left;
            }
            if ((flagx == -1) && (flagy == -1))
            {
                result = Direction.UpLeft;
            }
            return result;
        }

        public static int GetBack(int dir)
        {
            var result = Direction.Up;
            switch (dir)
            {
                case Direction.Up:
                    result = Direction.Down;
                    break;
                case Direction.Down:
                    result = Direction.Up;
                    break;
                case Direction.Left:
                    result = Direction.Right;
                    break;
                case Direction.Right:
                    result = Direction.Left;
                    break;
                case Direction.UpLeft:
                    result = Direction.DownRight;
                    break;
                case Direction.UpRight:
                    result = Direction.DownLeft;
                    break;
                case Direction.DownLeft:
                    result = Direction.UpRight;
                    break;
                case Direction.DownRight:
                    result = Direction.UpLeft;
                    break;
            }
            return result;
        }

        public static void GetBackPosition(short sX, short sY, int dir, ref short newX, ref short newY)
        {
            newX = sX;
            newY = sY;
            switch (dir)
            {
                case Direction.Up:
                    newY = (short)(newY + 1);
                    break;
                case Direction.Down:
                    newY = (short)(newY - 1);
                    break;
                case Direction.Left:
                    newX = (short)(newX + 1);
                    break;
                case Direction.Right:
                    newX = (short)(newX - 1);
                    break;
                case Direction.UpLeft:
                    newX = (short)(newX + 1);
                    newY = (short)(newY + 1);
                    break;
                case Direction.UpRight:
                    newX = (short)(newX - 1);
                    newY = (short)(newY + 1);
                    break;
                case Direction.DownLeft:
                    newX = (short)(newX + 1);
                    newY = (short)(newY - 1);
                    break;
                case Direction.DownRight:
                    newX = (short)(newX - 1);
                    newY = (short)(newY - 1);
                    break;
            }
        }

        public static void GetBackPosition2(short sX, short sY, int dir, ref short newX, ref short newY)
        {
            newX = sX;
            newY = sY;
            switch (dir)
            {
                case Direction.Up:
                    newY = (short)(newY + 2);
                    break;
                case Direction.Down:
                    newY = (short)(newY - 2);
                    break;
                case Direction.Left:
                    newX = (short)(newX + 2);
                    break;
                case Direction.Right:
                    newX = (short)(newX - 2);
                    break;
                case Direction.UpLeft:
                    newX = (short)(newX + 2);
                    newY = (short)(newY + 2);
                    break;
                case Direction.UpRight:
                    newX = (short)(newX - 2);
                    newY = (short)(newY + 2);
                    break;
                case Direction.DownLeft:
                    newX = (short)(newX + 2);
                    newY = (short)(newY - 2);
                    break;
                case Direction.DownRight:
                    newX = (short)(newX - 2);
                    newY = (short)(newY - 2);
                    break;
            }
        }

        public static void GetFrontPosition(short sX, short sY, int dir, ref short newX, ref short newY)
        {
            newX = sX;
            newY = sY;
            switch (dir)
            {
                case Direction.Up:
                    newY = (short)(newY - 1);
                    break;
                case Direction.Down:
                    newY = (short)(newY + 1);
                    break;
                case Direction.Left:
                    newX = (short)(newX - 1);
                    break;
                case Direction.Right:
                    newX = (short)(newX + 1);
                    break;
                case Direction.UpLeft:
                    newX = (short)(newX - 1);
                    newY = (short)(newY - 1);
                    break;
                case Direction.UpRight:
                    newX = (short)(newX + 1);
                    newY = (short)(newY - 1);
                    break;
                case Direction.DownLeft:
                    newX = (short)(newX - 1);
                    newY = (short)(newY + 1);
                    break;
                case Direction.DownRight:
                    newX = (short)(newX + 1);
                    newY = (short)(newY + 1);
                    break;
            }
        }

        public static byte GetFlyDirection(int sX, int sY, int ttx, int tty)
        {
            double fx = ttx - sX;
            double fy = tty - sY;
            var result = Direction.Down;
            if (fx == 0)
            {
                if (fy < 0)
                {
                    result = Direction.Up;
                }
                else
                {
                    result = Direction.Down;
                }
                return result;
            }
            if (fy == 0)
            {
                if (fx < 0)
                {
                    result = Direction.Left;
                }
                else
                {
                    result = Direction.Right;
                }
                return result;
            }
            if ((fx > 0) && (fy < 0))
            {
                if (-fy > fx * 2.5)
                {
                    result = Direction.Up;
                }
                else if (-fy < fx / 3)
                {
                    result = Direction.Right;
                }
                else
                {
                    result = Direction.UpRight;
                }
            }
            if ((fx > 0) && (fy > 0))
            {
                if (fy < fx / 3)
                {
                    result = Direction.Right;
                }
                else if (fy > fx * 2.5)
                {
                    result = Direction.Down;
                }
                else
                {
                    result = Direction.DownRight;
                }
            }
            if ((fx < 0) && (fy > 0))
            {
                if (fy < -fx / 3)
                {
                    result = Direction.Left;
                }
                else if (fy > -fx * 2.5)
                {
                    result = Direction.Down;
                }
                else
                {
                    result = Direction.DownLeft;
                }
            }
            if ((fx < 0) && (fy < 0))
            {
                if (-fy > -fx * 2.5)
                {
                    result = Direction.Up;
                }
                else if (-fy < -fx / 3)
                {
                    result = Direction.Left;
                }
                else
                {
                    result = Direction.UpLeft;
                }
            }
            return result;
        }

        public static int GetFlyDirection16(int sX, int sY, int ttx, int tty)
        {
            double fx = ttx - sX;
            double fy = tty - sY;
            int result = 0;
            if (fx == 0)
            {
                if (fy < 0)
                {
                    result = 0;
                }
                else
                {
                    result = 8;
                }
                return result;
            }
            if (fy == 0)
            {
                if (fx < 0)
                {
                    result = 12;
                }
                else
                {
                    result = 4;
                }
                return result;
            }
            if ((fx > 0) && (fy < 0))
            {
                result = 4;
                if (-fy > fx / 4)
                {
                    result = 3;
                }
                if (-fy > fx / 1.9)
                {
                    result = 2;
                }
                if (-fy > fx * 1.4)
                {
                    result = 1;
                }
                if (-fy > fx * 4)
                {
                    result = 0;
                }
            }
            if ((fx > 0) && (fy > 0))
            {
                result = 4;
                if (fy > fx / 4)
                {
                    result = 5;
                }
                if (fy > fx / 1.9)
                {
                    result = 6;
                }
                if (fy > fx * 1.4)
                {
                    result = 7;
                }
                if (fy > fx * 4)
                {
                    result = 8;
                }
            }
            if ((fx < 0) && (fy > 0))
            {
                result = 12;
                if (fy > -fx / 4)
                {
                    result = 11;
                }
                if (fy > -fx / 1.9)
                {
                    result = 10;
                }
                if (fy > -fx * 1.4)
                {
                    result = 9;
                }
                if (fy > -fx * 4)
                {
                    result = 8;
                }
            }
            if ((fx < 0) && (fy < 0))
            {
                result = 12;
                if (-fy > -fx / 4)
                {
                    result = 13;
                }
                if (-fy > -fx / 1.9)
                {
                    result = 14;
                }
                if (-fy > -fx * 1.4)
                {
                    result = 15;
                }
                if (-fy > -fx * 4)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static byte PrivDir(byte ndir)
        {
            byte result;
            if (ndir - 1 < 0)
            {
                result = 7;
            }
            else
            {
                result = (byte)(ndir - 1);
            }
            return result;
        }

        public static byte NextDir(byte ndir)
        {
            byte result;
            if (ndir + 1 > 7)
            {
                result = 0;
            }
            else
            {
                result = (byte)(ndir + 1);
            }
            return result;
        }

        public static int GetTakeOnPosition(ClientItem smode, ClientItem[] useItems, bool bPos)
        {
            int result = -1;
            switch (smode.Item.StdMode)
            {
                case 5:
                case 6:
                    result = ItemLocation.Weapon;
                    break;
                case 7:
                    result = ItemLocation.Charm;
                    break;
                case 10:
                case 11:
                    result = ItemLocation.Dress;
                    break;
                case 15:
                    result = ItemLocation.Helmet;
                    break;
                case 16:
                    result = 13;
                    break;
                case 12:
                case 13:
                    result = ItemLocation.Necklace;
                    break;
                case 22:
                case 23:
                    result = ItemLocation.Ringl;
                    if (bPos)
                    {
                        if (MShare.g_boTakeOnPos)
                        {
                            result = ItemLocation.Ringl;
                        }
                        else
                        {
                            result = ItemLocation.Ringr;
                        }
                        MShare.g_boTakeOnPos = !MShare.g_boTakeOnPos;
                    }
                    break;
                case 24:
                case 26:
                    result = ItemLocation.ArmRingr;
                    if (bPos)
                    {
                        if (MShare.g_boTakeOnPos)
                        {
                            result = ItemLocation.ArmRingr;
                        }
                        else
                        {
                            result = ItemLocation.ArmRingl;
                        }
                        MShare.g_boTakeOnPos = !MShare.g_boTakeOnPos;
                    }
                    break;
                case 30:
                    result = ItemLocation.RighThand;
                    break;
                // Modify the A .. B: 2 .. 4, 25
                case 2:
                case 25:
                    result = ItemLocation.Bujuk;
                    break;
                // Modify the A .. B: 41 .. 50
                case 41:
                    if (!(smode.Item.Shape >= 9 && smode.Item.Shape <= 45))
                    {
                        result = ItemLocation.Bujuk;
                    }
                    break;
                case 27: // ????
                    result = ItemLocation.Belt;
                    break;
                case 28:// ???
                    result = ItemLocation.Boots;
                    break;
                case 29:// ???
                    result = ItemLocation.Charm;
                    break;
            }
            return result;
        }

        public static int GetTakeOnPosition(ClientItem smode, ClientItem[] useItems)
        {
            return GetTakeOnPosition(smode, useItems, false);
        }

        public static void AddChangeFace(int recogid)
        {
            MShare.g_ChangeFaceReadyList.Add(recogid);
        }

        public static void DelChangeFace(int recogid)
        {
            int i = MShare.g_ChangeFaceReadyList.IndexOf(recogid);
            if (i > -1)
            {
                MShare.g_ChangeFaceReadyList.RemoveAt(i);
            }
        }

        public static bool IsChangingFace(int recogid)
        {
            return MShare.g_ChangeFaceReadyList.IndexOf(recogid) > -1;
        }

        public static bool ChangeItemCount(int mindex, short count, short msgNum, string iname)
        {
            bool result = false;
            //for (i = MShare.g_TIItems.GetLowerBound(0); i <= MShare.g_TIItems..Length(0); i++)
            //{
            //    if ((MShare.g_TIItems[i].Item.Item.Name == iname) && (MShare.g_TIItems[i].Item.Item.Overlap > 0) && (MShare.g_TIItems[i].Item.MakeIndex == mindex))
            //    {
            //        MShare.g_TIItems[i].Item.Dura = Count;
            //        result = true;
            //    }
            //}
            //for (i = MShare.g_spItems.GetLowerBound(0); i <= MShare.g_spItems..Length(0); i++)
            //{
            //    if ((MShare.g_spItems[i].Item.Item.Name == iname) && (MShare.g_spItems[i].Item.Item.Overlap > 0) && (MShare.g_spItems[i].Item.MakeIndex == mindex))
            //    {
            //        MShare.g_spItems[i].Item.Dura = Count;
            //        result = true;
            //    }
            //}
            //if ((MShare.g_SellDl.Item.Item.Name == iname) && (MShare.g_SellDl.Item.Item.Overlap > 0) && (MShare.g_SellDlgItem.MakeIndex == mindex))
            //{
            //    MShare.g_SellDlgItem.Dura = Count;
            //    result = true;
            //}
            //if ((MShare.g_Eatin.Item.Item.Name == iname) && (MShare.g_Eatin.Item.Item.Overlap > 0) && (MShare.g_EatingItem.MakeIndex == mindex))
            //{
            //    MShare.g_EatingItem.Dura = Count;
            //    result = true;
            //}
            //if ((MShare.g_MovingItem.Item.Item.Name == iname) && (MShare.g_MovingItem.Item.Item.Overlap > 0) && (MShare.g_MovingItem.Item.MakeIndex == mindex))
            //{
            //    MShare.g_MovingItem.Item.Dura = Count;
            //    result = true;
            //}
            //if ((MShare.g_WaitingUseItem.Item.Item.Name == iname) && (MShare.g_WaitingUseItem.Item.Item.Overlap > 0) && (MShare.g_WaitingUseItem.Item.MakeIndex == mindex))
            //{
            //    MShare.g_WaitingUseItem.Item.Dura = Count;
            //    result = true;
            //}
            //MShare.g_WaitingUseItem.Item.Item.Name = "";
            //for (i = 0; i < BotConst.MAXBAGITEMCL; i++)
            //{
            //    if ((MShare.g_ItemArr[i].MakeIndex == mindex) && (MShare.g_ItemArr[i].Item.Name == iname) && (MShare.g_ItemArr[i].Item.Overlap > 0))
            //    {
            //        if (Count < 1)
            //        {
            //            MShare.g_ItemArr[i].Item.Name = "";
            //            Count = 0;
            //        }
            //        MShare.g_ItemArr[i].Dura = Count;
            //        result = true;
            //        break;
            //    }
            //}
            //ArrangeItembag();
            //if ((result == false) && (!MShare.g_boDealEnd))
            //{
            //    for (i = 0; i < 10; i++)
            //    {
            //        if ((MShare.g_DealItems[i].Item.Name == iname) && (MShare.g_DealItems[i].Item.Overlap > 0) && (MShare.g_DealItems[i].MakeIndex == mindex))
            //        {
            //            if (Count < 1)
            //            {
            //                MShare.g_DealItems[i].Item.Name = "";
            //                Count = 0;
            //            }
            //            MShare.g_DealItems[i].Dura = Count;
            //            result = true;
            //            break;
            //        }
            //    }
            //}
            //if ((result == false) && (!MShare.g_boDealEnd))
            //{
            //    for (i = 0; i <= 19; i++)
            //    {
            //        if ((MShare.g_DealRemoteItems[i].Item.Name == iname) && (MShare.g_DealRemoteItems[i].Item.Overlap > 0) && (MShare.g_DealRemoteItems[i].MakeIndex == mindex))
            //        {
            //            if (Count < 1)
            //            {
            //                MShare.g_DealRemoteItems[i].Item.Name = "";
            //                Count = 0;
            //            }
            //            MShare.g_DealRemoteItems[i].Dura = Count;
            //            result = true;
            //            break;
            //        }
            //    }
            //}
            //if (MsgNum == 1)
            //{
            //    ClMain.Units.ClMain.DScreen.AddSysMsg(iname + " ??????");
            //}
            return result;
        }

        public static bool SellItemProg(ushort remain, short sellcnt)
        {
            bool result = false;
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if ((MShare.ItemArr[i].MakeIndex == MShare.g_SellDlgItemSellWait.Item.MakeIndex) && (MShare.ItemArr[i].Item.Name == MShare.g_SellDlgItemSellWait.Item.Item.Name))
                {
                    if (remain < 1)
                    {
                        MShare.ItemArr[i].Item.Name = "";
                        remain = 0;
                    }
                    MShare.ItemArr[i].Dura = remain;
                    result = true;
                }
            }
            return result;
        }

        public static bool DelCountItemBag(string iname, int iindex, short count)
        {
            bool result = false;
            for (var i = BotConst.MaxBagItemcl - 1; i >= 0; i--)
            {
                if (MShare.ItemArr[i].Item.Name == iname)
                {
                    if (MShare.ItemArr[i].MakeIndex == iindex)
                    {
                        result = true;
                        break;
                    }
                }
            }
            ArrangeItembag();
            return result;
        }

        public static void ResultDealItem(ClientItem ci, int mindex, ushort count)
        {
            for (var i = 0; i < 10; i++)
            {
                if (MShare.g_DealItems[i].Item.Name == "")
                {
                    MShare.g_DealItems[i] = ci;
                    MShare.g_DealItems[i].MakeIndex = mindex;
                    break;
                }
            }
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if ((MShare.ItemArr[i].Item.Name == ci.Item.Name) && (MShare.ItemArr[i].MakeIndex == ci.MakeIndex))
                {
                    if (count < 1)
                    {
                        MShare.ItemArr[i].Item.Name = "";
                        count = 0;
                    }
                    MShare.ItemArr[i].Dura = count;
                }
            }
        }
    }
}
