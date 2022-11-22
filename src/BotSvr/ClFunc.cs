using System;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Packets.ClientPackets;

namespace BotSvr
{
    public class ClFunc
    {
        private static readonly IList<ClientItem> DropItems = null;

        public static void GetNextHitPosition(short sX, short sY, ref short NewX, ref short NewY)
        {
            int dir;
            dir = GetNextDirection(sX, sY, NewX, NewY);
            NewX = sX;
            NewY = sY;
            switch (dir)
            {
                case Grobal2.DR_UP:
                    NewY = (short)(NewY - 2);
                    break;
                case Grobal2.DR_DOWN:
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_LEFT:
                    NewX = (short)(NewX + 2);
                    break;
                case Grobal2.DR_RIGHT:
                    NewX = (short)(NewX - 2);
                    break;
                case Grobal2.DR_UPLEFT:
                    NewX = (short)(NewX - 2);
                    NewY = (short)(NewY - 2);
                    break;
                case Grobal2.DR_UPRIGHT:
                    NewX = (short)(NewX + 2);
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    NewX = (short)(NewX - 2);
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    NewX = (short)(NewX + 2);
                    NewY = (short)(NewY + 2);
                    break;
            }
        }

        public static void ClearBag()
        {
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                MShare.g_ItemArr[i].Item.Name = "";
            }
        }

        public static void AddItemBag(ClientItem cu, int idx = 0)
        {
            bool InputCheck = false;
            if (cu == null)
            {
                return;
            }
            if (cu.Item.Name == "")
            {
                return;
            }
            if (idx >= 0)
            {
                if (MShare.g_ItemArr[idx] == null || MShare.g_ItemArr[idx].Item.Name == "")
                {
                    MShare.g_ItemArr[idx] = cu;
                    return;
                }
            }
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.g_ItemArr[i].MakeIndex == cu.MakeIndex) && (MShare.g_ItemArr[i].Item.Name == cu.Item.Name))
                {
                    return;
                }
            }
            if (cu.Item.Name == "")
            {
                return;
            }
            if (cu.Item.StdMode <= 3)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (MShare.g_ItemArr[i] == null || MShare.g_ItemArr[i].Item.Name == "")
                    {
                        MShare.g_ItemArr[i] = cu;
                        return;
                    }
                }
            }
            for (var i = 6; i < MShare.MAXBAGITEMCL; i++)
            {
                if ((MShare.g_ItemArr[i].Item.Name == cu.Item.Name) && (MShare.g_ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.g_ItemArr[i].Dura = (ushort)(MShare.g_ItemArr[i].Dura + cu.Dura);
                    cu.Dura = 0;
                    InputCheck = true;
                }
            }
            if (!InputCheck)
            {
                for (var i = 6; i < MShare.MAXBAGITEMCL; i++)
                {
                    if (MShare.g_ItemArr[i].Item.Name == "")
                    {
                        MShare.g_ItemArr[i] = cu;
                        break;
                    }
                }
            }
            ArrangeItembag();
        }

        public static void UpdateItemBag(ClientItem cu)
        {
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 0; i--)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.g_ItemArr[i].Item.Name == cu.Item.Name) && (MShare.g_ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.g_ItemArr[i] = cu;
                    break;
                }
            }
        }

        public static void UpdateBagStallItem(ClientItem cu, byte ststus)
        {
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 6; i--)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.g_ItemArr[i].Item.Name == cu.Item.Name) && (MShare.g_ItemArr[i].MakeIndex == cu.MakeIndex))
                {
                    MShare.g_ItemArr[i].Item.NeedIdentify = ststus;
                    break;
                }
            }
        }

        public static bool FillBagStallItem(byte ststus)
        {
            bool result = false;
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 6; i--)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.g_ItemArr[i].Item.Name != "") && (MShare.g_ItemArr[i].Item.NeedIdentify != ststus))
                {
                    MShare.g_ItemArr[i].Item.NeedIdentify = ststus;
                    result = true;
                }
            }
            return result;
        }

        public static void DelItemBag(string iname, int iindex)
        {
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 0; i--)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if ((MShare.g_ItemArr[i].Item.Name == iname) && (MShare.g_ItemArr[i].MakeIndex == iindex))
                {
                    break;
                }
            }
            if (MShare.g_MySelf != null)
            {
                for (var i = 10 - 1; i >= 0; i--)
                {
                    if ((MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name == iname) && (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].MakeIndex == iindex))
                    {
                        break;
                    }
                }
            }
            ArrangeItembag();
        }

        public static void ArrangeItembag()
        {
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if (MShare.g_ItemArr[i].Item.Name != "")
                {
                    for (var k = i + 1; k < MShare.MAXBAGITEMCL; k++)
                    {
                        if (MShare.g_ItemArr[k] == null)
                        {
                            continue;
                        }
                        if ((MShare.g_ItemArr[i].Item.Name == MShare.g_ItemArr[k].Item.Name) && (MShare.g_ItemArr[i].MakeIndex == MShare.g_ItemArr[k].MakeIndex))// ????????
                        {
                            //if (MShare.g_ItemArr[i].Item.Overlap > 0)
                            //{
                            //    MShare.g_ItemArr[i].Dura = MShare.g_ItemArr[i].Dura + MShare.g_ItemArr[k].Dura;
                            //}
                        }
                    }
                    if (MShare.g_MovingItem != null)
                    {
                        if ((MShare.g_ItemArr[i].Item.Name == MShare.g_MovingItem.Item.Item.Name) && (MShare.g_ItemArr[i].MakeIndex == MShare.g_MovingItem.Item.MakeIndex))
                        {
                            MShare.g_MovingItem.Index = 0;
                            MShare.g_MovingItem.Item.Item.Name = "";
                            MShare.g_boItemMoving = false;
                        }
                    }
                }
            }
            for (var i = 46; i < MShare.MAXBAGITEMCL; i++)
            {
                if (MShare.g_ItemArr[i] == null)
                {
                    continue;
                }
                if (MShare.g_ItemArr[i].Item.Name != "")
                {
                    for (var k = 6; k < 45; k++)
                    {
                        if (MShare.g_ItemArr[k].Item.Name == "")
                        {
                            MShare.g_ItemArr[k] = MShare.g_ItemArr[i];
                            MShare.g_ItemArr[i].Item.Name = "";
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

        public static ClientItem GetDrosItem(string iname, int MakeIndex)
        {
            ClientItem result = null;
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == MakeIndex))
                {
                    result = DropItems[i];
                    break;
                }
            }
            return result;
        }

        public static void DelDroItemItem(string iname, int MakeIndex)
        {
            for (var i = 0; i < DropItems.Count; i++)
            {
                if ((DropItems[i].Item.Name == iname) && (DropItems[i].MakeIndex == MakeIndex))
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
                if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name == "")
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
                if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name != "")
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
                if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name != "")
                {
                    if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].MakeIndex == ci.MakeIndex)
                    {
                        MShare.g_MySelf.m_StallMgr.mBlock.Items[i] = ci;
                        result = true;
                        return result;
                    }
                }
            }
            for (var i = 0; i < 10; i++)
            {
                if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name == "")
                {
                    MShare.g_MySelf.m_StallMgr.mBlock.Items[i] = ci;
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
                if ((MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name == ci.Item.Name) && (ci.MakeIndex == MShare.g_MySelf.m_StallMgr.mBlock.Items[i].MakeIndex))
                {
                    MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name = "";
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

        public static void GetNextPosXY(byte dir, ref short X, ref short Y)
        {
            switch (dir)
            {
                case Grobal2.DR_UP:
                    Y = (short)(Y - 1);
                    break;
                case Grobal2.DR_UPRIGHT:
                    X = (short)(X + 1);
                    Y = (short)(Y - 1);
                    break;
                case Grobal2.DR_RIGHT:
                    X = (short)(X + 1);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    X = (short)(X + 1);
                    Y = (short)(Y + 1);
                    break;
                case Grobal2.DR_DOWN:
                    Y = (short)(Y + 1);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    X = (short)(X - 1);
                    Y = (short)(Y + 1);
                    break;
                case Grobal2.DR_LEFT:
                    X = (short)(X - 1);
                    break;
                case Grobal2.DR_UPLEFT:
                    X = (short)(X - 1);
                    Y = (short)(Y - 1);
                    break;
            }
        }

        public static void GetNextRunXY(int dir, ref short X, ref short Y)
        {
            switch (dir)
            {
                case Grobal2.DR_UP:
                    Y = (short)(Y - 2);
                    break;
                case Grobal2.DR_UPRIGHT:
                    X = (short)(X + 2);
                    Y = (short)(Y - 2);
                    break;
                case Grobal2.DR_RIGHT:
                    X = (short)(X + 2);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    X = (short)(X + 2);
                    Y = (short)(Y + 2);
                    break;
                case Grobal2.DR_DOWN:
                    Y = (short)(Y + 2);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    X = (short)(X - 2);
                    Y = (short)(Y + 2);
                    break;
                case Grobal2.DR_LEFT:
                    X = (short)(X - 2);
                    break;
                case Grobal2.DR_UPLEFT:
                    X = (short)(X - 2);
                    Y = (short)(Y - 2);
                    break;
            }
        }

        public static void GetNextHorseRunXY(byte dir, ref short X, ref short Y)
        {
            switch (dir)
            {
                case Grobal2.DR_UP:
                    Y = (short)(Y - 3);
                    break;
                case Grobal2.DR_UPRIGHT:
                    X = (short)(X + 3);
                    Y = (short)(Y - 3);
                    break;
                case Grobal2.DR_RIGHT:
                    X = (short)(X + 3);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    X = (short)(X + 3);
                    Y = (short)(Y + 3);
                    break;
                case Grobal2.DR_DOWN:
                    Y = (short)(Y + 3);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    X = (short)(X - 3);
                    Y = (short)(Y + 3);
                    break;
                case Grobal2.DR_LEFT:
                    X = (short)(X - 3);
                    break;
                case Grobal2.DR_UPLEFT:
                    X = (short)(X - 3);
                    Y = (short)(Y - 3);
                    break;
            }
        }

        public static byte GetNextDirection(int sX, int sY, int dx, int dy)
        {
            int flagx;
            int flagy;
            byte result = Grobal2.DR_DOWN;
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
                result = Grobal2.DR_UP;
            }
            if ((flagx == 1) && (flagy == -1))
            {
                result = Grobal2.DR_UPRIGHT;
            }
            if ((flagx == 1) && (flagy == 0))
            {
                result = Grobal2.DR_RIGHT;
            }
            if ((flagx == 1) && (flagy == 1))
            {
                result = Grobal2.DR_DOWNRIGHT;
            }
            if ((flagx == 0) && (flagy == 1))
            {
                result = Grobal2.DR_DOWN;
            }
            if ((flagx == -1) && (flagy == 1))
            {
                result = Grobal2.DR_DOWNLEFT;
            }
            if ((flagx == -1) && (flagy == 0))
            {
                result = Grobal2.DR_LEFT;
            }
            if ((flagx == -1) && (flagy == -1))
            {
                result = Grobal2.DR_UPLEFT;
            }
            return result;
        }

        public static int GetBack(int dir)
        {
            int result = Grobal2.DR_UP;
            switch (dir)
            {
                case Grobal2.DR_UP:
                    result = Grobal2.DR_DOWN;
                    break;
                case Grobal2.DR_DOWN:
                    result = Grobal2.DR_UP;
                    break;
                case Grobal2.DR_LEFT:
                    result = Grobal2.DR_RIGHT;
                    break;
                case Grobal2.DR_RIGHT:
                    result = Grobal2.DR_LEFT;
                    break;
                case Grobal2.DR_UPLEFT:
                    result = Grobal2.DR_DOWNRIGHT;
                    break;
                case Grobal2.DR_UPRIGHT:
                    result = Grobal2.DR_DOWNLEFT;
                    break;
                case Grobal2.DR_DOWNLEFT:
                    result = Grobal2.DR_UPRIGHT;
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    result = Grobal2.DR_UPLEFT;
                    break;
            }
            return result;
        }

        public static void GetBackPosition(short sX, short sY, int dir, ref short NewX, ref short NewY)
        {
            NewX = sX;
            NewY = sY;
            switch (dir)
            {
                case Grobal2.DR_UP:
                    NewY = (short)(NewY + 1);
                    break;
                case Grobal2.DR_DOWN:
                    NewY = (short)(NewY - 1);
                    break;
                case Grobal2.DR_LEFT:
                    NewX = (short)(NewX + 1);
                    break;
                case Grobal2.DR_RIGHT:
                    NewX = (short)(NewX - 1);
                    break;
                case Grobal2.DR_UPLEFT:
                    NewX = (short)(NewX + 1);
                    NewY = (short)(NewY + 1);
                    break;
                case Grobal2.DR_UPRIGHT:
                    NewX = (short)(NewX - 1);
                    NewY = (short)(NewY + 1);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    NewX = (short)(NewX + 1);
                    NewY = (short)(NewY - 1);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    NewX = (short)(NewX - 1);
                    NewY = (short)(NewY - 1);
                    break;
            }
        }

        public static void GetBackPosition2(short sX, short sY, int dir, ref short NewX, ref short NewY)
        {
            NewX = sX;
            NewY = sY;
            switch (dir)
            {
                case Grobal2.DR_UP:
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_DOWN:
                    NewY = (short)(NewY - 2);
                    break;
                case Grobal2.DR_LEFT:
                    NewX = (short)(NewX + 2);
                    break;
                case Grobal2.DR_RIGHT:
                    NewX = (short)(NewX - 2);
                    break;
                case Grobal2.DR_UPLEFT:
                    NewX = (short)(NewX + 2);
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_UPRIGHT:
                    NewX = (short)(NewX - 2);
                    NewY = (short)(NewY + 2);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    NewX = (short)(NewX + 2);
                    NewY = (short)(NewY - 2);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    NewX = (short)(NewX - 2);
                    NewY = (short)(NewY - 2);
                    break;
            }
        }

        public static void GetFrontPosition(short sX, short sY, int dir, ref short NewX, ref short NewY)
        {
            NewX = sX;
            NewY = sY;
            switch (dir)
            {
                case Grobal2.DR_UP:
                    NewY = (short)(NewY - 1);
                    break;
                case Grobal2.DR_DOWN:
                    NewY = (short)(NewY + 1);
                    break;
                case Grobal2.DR_LEFT:
                    NewX = (short)(NewX - 1);
                    break;
                case Grobal2.DR_RIGHT:
                    NewX = (short)(NewX + 1);
                    break;
                case Grobal2.DR_UPLEFT:
                    NewX = (short)(NewX - 1);
                    NewY = (short)(NewY - 1);
                    break;
                case Grobal2.DR_UPRIGHT:
                    NewX = (short)(NewX + 1);
                    NewY = (short)(NewY - 1);
                    break;
                case Grobal2.DR_DOWNLEFT:
                    NewX = (short)(NewX - 1);
                    NewY = (short)(NewY + 1);
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    NewX = (short)(NewX + 1);
                    NewY = (short)(NewY + 1);
                    break;
            }
        }

        public static int GetFlyDirection(int sX, int sY, int ttx, int tty)
        {
            double fx = ttx - sX;
            double fy = tty - sY;
            int result = Grobal2.DR_DOWN;
            if (fx == 0)
            {
                if (fy < 0)
                {
                    result = Grobal2.DR_UP;
                }
                else
                {
                    result = Grobal2.DR_DOWN;
                }
                return result;
            }
            if (fy == 0)
            {
                if (fx < 0)
                {
                    result = Grobal2.DR_LEFT;
                }
                else
                {
                    result = Grobal2.DR_RIGHT;
                }
                return result;
            }
            if ((fx > 0) && (fy < 0))
            {
                if (-fy > fx * 2.5)
                {
                    result = Grobal2.DR_UP;
                }
                else if (-fy < fx / 3)
                {
                    result = Grobal2.DR_RIGHT;
                }
                else
                {
                    result = Grobal2.DR_UPRIGHT;
                }
            }
            if ((fx > 0) && (fy > 0))
            {
                if (fy < fx / 3)
                {
                    result = Grobal2.DR_RIGHT;
                }
                else if (fy > fx * 2.5)
                {
                    result = Grobal2.DR_DOWN;
                }
                else
                {
                    result = Grobal2.DR_DOWNRIGHT;
                }
            }
            if ((fx < 0) && (fy > 0))
            {
                if (fy < -fx / 3)
                {
                    result = Grobal2.DR_LEFT;
                }
                else if (fy > -fx * 2.5)
                {
                    result = Grobal2.DR_DOWN;
                }
                else
                {
                    result = Grobal2.DR_DOWNLEFT;
                }
            }
            if ((fx < 0) && (fy < 0))
            {
                if (-fy > -fx * 2.5)
                {
                    result = Grobal2.DR_UP;
                }
                else if (-fy < -fx / 3)
                {
                    result = Grobal2.DR_LEFT;
                }
                else
                {
                    result = Grobal2.DR_UPLEFT;
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

        public static int GetTakeOnPosition(ClientItem smode, ClientItem[] UseItems, bool bPos)
        {
            int result = -1;
            switch (smode.Item.StdMode)
            {
                case 5:
                case 6:
                    result = Grobal2.U_WEAPON;
                    break;
                case 7:
                    result = Grobal2.U_CHARM;
                    break;
                case 10:
                case 11:
                    result = Grobal2.U_DRESS;
                    break;
                case 15:
                    result = Grobal2.U_HELMET;
                    break;
                case 16:
                    result = 13;
                    break;
                case 12:
                case 13:
                    result = Grobal2.U_NECKLACE;
                    break;
                case 22:
                case 23:
                    result = Grobal2.U_RINGL;
                    if (bPos)
                    {
                        if (MShare.g_boTakeOnPos)
                        {
                            result = Grobal2.U_RINGL;
                        }
                        else
                        {
                            result = Grobal2.U_RINGR;
                        }
                        MShare.g_boTakeOnPos = !MShare.g_boTakeOnPos;
                    }
                    break;
                case 24:
                case 26:
                    result = Grobal2.U_ARMRINGR;
                    if (bPos)
                    {
                        if (MShare.g_boTakeOnPos)
                        {
                            result = Grobal2.U_ARMRINGR;
                        }
                        else
                        {
                            result = Grobal2.U_ARMRINGL;
                        }
                        MShare.g_boTakeOnPos = !MShare.g_boTakeOnPos;
                    }
                    break;
                case 30:
                    result = Grobal2.U_RIGHTHAND;
                    break;
                // Modify the A .. B: 2 .. 4, 25
                case 2:
                case 25:
                    result = Grobal2.U_BUJUK;
                    break;
                // Modify the A .. B: 41 .. 50
                case 41:
                    if (!(smode.Item.Shape >= 9 && smode.Item.Shape <= 45))
                    {
                        result = Grobal2.U_BUJUK;
                    }
                    break;
                case 27: // ????
                    result = Grobal2.U_BELT;
                    break;
                case 28:// ???
                    result = Grobal2.U_BOOTS;
                    break;
                case 29:// ???
                    result = Grobal2.U_CHARM;
                    break;
            }
            return result;
        }

        public static int GetTakeOnPosition(ClientItem smode, ClientItem[] UseItems)
        {
            return GetTakeOnPosition(smode, UseItems, false);
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

        public static bool ChangeItemCount(int mindex, short Count, short MsgNum, string iname)
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
            //for (i = 0; i < MShare.MAXBAGITEMCL; i++)
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
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if ((MShare.g_ItemArr[i].MakeIndex == MShare.g_SellDlgItemSellWait.Item.MakeIndex) && (MShare.g_ItemArr[i].Item.Name == MShare.g_SellDlgItemSellWait.Item.Item.Name))
                {
                    if (remain < 1)
                    {
                        MShare.g_ItemArr[i].Item.Name = "";
                        remain = 0;
                    }
                    MShare.g_ItemArr[i].Dura = remain;
                    result = true;
                }
            }
            return result;
        }

        public static bool DelCountItemBag(string iname, int iindex, short Count)
        {
            bool result = false;
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 0; i--)
            {
                if (MShare.g_ItemArr[i].Item.Name == iname)
                {
                    if (MShare.g_ItemArr[i].MakeIndex == iindex)
                    {
                        result = true;
                        break;
                    }
                }
            }
            ArrangeItembag();
            return result;
        }

        public static void ResultDealItem(ClientItem ci, int mindex, ushort Count)
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
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if ((MShare.g_ItemArr[i].Item.Name == ci.Item.Name) && (MShare.g_ItemArr[i].MakeIndex == ci.MakeIndex))
                {
                    if (Count < 1)
                    {
                        MShare.g_ItemArr[i].Item.Name = "";
                        Count = 0;
                    }
                    MShare.g_ItemArr[i].Dura = Count;
                }
            }
        }
    }
}
