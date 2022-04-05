using System;
using System.Collections;

namespace RobotSvr
{
    public class HeroActor
    {
        public static long g_hinttick1 = 0;
        public static long g_hinttick2 = 0;
        public const int overdisc = 22;
        // 待处理节点入队列, 依靠对目的地估价距离插入排序
        public static bool CanNextSpell()
        {
            bool result;
            result = false;
            if (MShare.g_boSpeedRate)
            {
                if (MShare.GetTickCount() - MShare.g_dwLatestSpellTick > (MShare.g_dwSpellTime + MShare.g_dwMagicDelayTime - ((long)MShare.g_MagSpeedRate) * 20))
                {
                    result = true;
                }
            }
            else
            {
                if (MShare.GetTickCount() - MShare.g_dwLatestSpellTick > (MShare.g_dwSpellTime + MShare.g_dwMagicDelayTime))
                {
                    result = true;
                }
            }
            return result;
        }

        public static void enter_queue(Tree Node, int F)
        {
            Link P;
            Link Father;
            Link q;
            P = MShare.g_APQueue;
            Father = P;
            while ((F > P.F))
            {
                Father = P;
                P = P.Next;
                if (P == null)
                {
                    break;
                }
            }
            q = new Link();
            q.F = F;
            q.Node = Node;
            q.Next = P;
            Father.Next = q;
        }

        // 将离目的地估计最近的方案出队列
        public static Tree get_from_queue()
        {
            Tree result;
            Tree bestchoice;
            Link Next;
            bestchoice = MShare.g_APQueue.Next.Node;
            Next = MShare.g_APQueue.Next.Next;
            //@ Unsupported function or procedure: 'Dispose'
            Dispose(MShare.g_APQueue.Next);
            MShare.g_APQueue.Next = Next;
            result = bestchoice;
            return result;
        }

        // 释放申请过的所有节点
        public static void FreeTree()
        {
            Link P;
            while ((MShare.g_APQueue != null))
            {
                P = MShare.g_APQueue;
                if (P.Node != null)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(P.Node);
                }
                P.Node = null;
                MShare.g_APQueue = MShare.g_APQueue.Next;
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(P);
            }
        }

        // 估价函数,估价 x,y 到目的地的距离,估计值必须保证比实际值小
        public static int judge(int X, int Y, int end_x, int end_y)
        {
            int result;
            result = Math.Abs(end_x - X) + Math.Abs(end_y - Y);
            return result;
        }

        public bool Trytile_has(int X, int Y, int H)
        {
            bool result;
            int cx;
            int cy;
            result = true;
            cx = X -ClMain.Map.m_nBlockLeft;
            cy = Y -ClMain.Map.m_nBlockTop;
            if ((cx > MShare.MAXX * 3) || (cy > MShare.MAXY * 3))
            {
                return result;
            }
            if ((cx < 0) || (cy < 0))
            {
                return result;
            }
            if ((long)H < MShare.g_APPass[cx, cy])
            {
                result = false;
            }
            return result;
        }

        public static bool Trytile(int X, int Y, int end_x, int end_y, Tree Father, byte dir)
        {
            bool result;
            Tree P;
            int H;
            result = false;
            if (!ClMain.Map.CanMove(X, Y))
            {
                return result;
            }
            P = Father;
            while ((P != null))
            {
                if ((X == P.X) && (Y == P.Y))
                {
                    return result;
                }
                // 如果 (x,y) 曾经经过,失败
                P = P.Father;
            }
            H = Father.H + 1;
            if (Trytile_has(X, Y, H))
            {
                return result;
            }
            // 如果曾经有更好的方案移动到 (x,y) 失败
            MShare.g_APPass[X -ClMain.Map.m_nBlockLeft, Y -ClMain.Map.m_nBlockTop] = H;
            // 记录这次到 (x,y) 的距离为历史最佳距离
            P = new Tree();
            P.Father = Father;
            P.H = Father.H + 1;
            P.X = X;
            P.Y = Y;
            P.Dir = dir;
            enter_queue(P, P.H + judge(X, Y, end_x, end_y));
            result = true;
            return result;
        }

        // 路径寻找主函数
        public static void AP_findpath(int Startx, int Starty, int end_x, int end_y)
        {
            Tree Root;
            Tree P;
            int i;
            int X;
            int Y;
            int dir;
            TFindNode Temp;
            if (!ClMain.Map.CanMove(end_x, end_y))
            {
                return;
            }
            //@ Unsupported function or procedure: 'FillChar'
            FillChar(MShare.g_APPass);            Init_Queue();
            Root = new Tree();
            Root.X = Startx;
            Root.Y = Starty;
            Root.H = 0;
            Root.Father = null;
            enter_queue(Root, judge(Startx, Starty, end_x, end_y));
            while (true)
            {
                Root = get_from_queue();
                // 将第一个弹出
                if (Root == null)
                {
                    break;
                }
                X = Root.X;
                Y = Root.Y;
                if ((X == end_x) && (Y == end_y))
                {
                    break;
                }
                Trytile(X, Y - 1, end_x, end_y, Root, 0);
                // 尝试向上移动
                Trytile(X + 1, Y - 1, end_x, end_y, Root, 1);
                // 尝试向右上移动
                Trytile(X + 1, Y, end_x, end_y, Root, 2);
                // 尝试向右移动
                Trytile(X + 1, Y + 1, end_x, end_y, Root, 3);
                // 尝试向右下移动
                Trytile(X, Y + 1, end_x, end_y, Root, 4);
                // 尝试向下移动
                Trytile(X - 1, Y + 1, end_x, end_y, Root, 5);
                // 尝试向左下移动
                Trytile(X - 1, Y, end_x, end_y, Root, 6);
                // 尝试向左移动
                Trytile(X - 1, Y - 1, end_x, end_y, Root, 7);
            // 尝试向左上移动
            }
            for (i = MShare.g_APPathList.Count - 1; i >= 0; i-- )
            {
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(((TFindNode)(MShare.g_APPathList[i])));
            }
            MShare.g_APPathList.Clear();
            if (Root == null)
            {
                FreeTree();
                // 内存泄露???
                return;
            }
            Temp = new TFindNode();
            Temp.X = Root.X;
            Temp.Y = Root.Y;
            MShare.g_APPathList.Add(Temp);
            dir = Root.Dir;
            P = Root;
            Root = Root.Father;
            while ((Root != null))
            {
                if (dir != Root.Dir)
                {
                    Temp = new TFindNode();
                    Temp.X = P.X;
                    Temp.Y = P.Y;
                    MShare.g_APPathList.Insert(0, Temp);
                    dir = Root.Dir;
                }
                P = Root;
                Root = Root.Father;
            }
            FreeTree();
        }

        public static int RandomRange(int AFrom, int ATo)
        {
            int result;
            if (AFrom > ATo)
            {
                result = (new System.Random(AFrom - ATo)).Next() + ATo;
            }
            else
            {
                result = (new System.Random(ATo - AFrom)).Next() + AFrom;
            }
            return result;
        }

        public static void Init_Queue()
        {
            int i;
            FreeTree();
            // 内存泄露???
            if (MShare.g_APQueue != null)
            {
                if (MShare.g_APQueue.Next != null)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(MShare.g_APQueue.Next);
                }
                MShare.g_APQueue.Next = null;
                if (MShare.g_APQueue.Node != null)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(MShare.g_APQueue.Node);
                }
                MShare.g_APQueue.Node = null;
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(MShare.g_APQueue);
                MShare.g_APQueue = null;
            }
            MShare.g_APQueue = new Link();
            MShare.g_APQueue.Node = null;
            MShare.g_APQueue.F =  -1;
            MShare.g_APQueue.Next = new Link();
            MShare.g_APQueue.Next.F = 0xFFFFFFF;
            MShare.g_APQueue.Next.Node = null;
            MShare.g_APQueue.Next.Next = null;
            for (i = MShare.g_APPathList.Count - 1; i >= 0; i-- )
            {
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(((TFindNode)(MShare.g_APPathList[i])));
            }
            MShare.g_APPathList.Clear();
        }

        public static void Init_Queue2()
        {
            int i;
            FreeTree();
            // 内存泄露???
            if (MShare.g_APQueue != null)
            {
                if (MShare.g_APQueue.Next != null)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(MShare.g_APQueue.Next);
                }
                MShare.g_APQueue.Next = null;
                if (MShare.g_APQueue.Node != null)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(MShare.g_APQueue.Node);
                }
                MShare.g_APQueue.Node = null;
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(MShare.g_APQueue);
                MShare.g_APQueue = null;
            }
            for (i = MShare.g_APPathList.Count - 1; i >= 0; i-- )
            {
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(((TFindNode)(MShare.g_APPathList[i])));
            }
            MShare.g_APPathList.Clear();
        }

        public static bool IsBackToSafeZone(ref int ret)
        {
            bool result;
            int i;
            bool has;
            result = false;
            ret = 0;
            if (MShare.g_gcAss[1])
            {
                // 红没有回城
                has = false;
                for (i = 0; i < MShare.MAXBAGITEMCL; i ++ )
                {
                    if ((MShare.g_ItemArr[i].s.Name != "") && (MShare.g_ItemArr[i].s.AC > 0) && (MShare.g_ItemArr[i].s.StdMode == 0))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    ret = 1;
                    result = true;
                    return result;
                }
            }
            if (MShare.g_gcAss[2])
            {
                // 蓝没有回城
                has = false;
                for (i = 0; i < MShare.MAXBAGITEMCL; i ++ )
                {
                    if ((MShare.g_ItemArr[i].s.Name != "") && (MShare.g_ItemArr[i].s.MAC > 0) && (MShare.g_ItemArr[i].s.StdMode == 0))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    ret = 2;
                    result = true;
                    return result;
                }
            }
            // 包裹满没有回城
            if (MShare.g_gcAss[4])
            {
                has = false;
                for (i = 0; i <= 45; i ++ )
                {
                    if ((MShare.g_ItemArr[i].s.Name == ""))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    ret = 3;
                    result = true;
                    return result;
                }
            }
            // 符没有回城
            if (MShare.g_gcAss[3])
            {
                has = false;
                for (i = 0; i < MShare.MAXBAGITEMCL; i ++ )
                {
                    if ((MShare.g_ItemArr[i].s.StdMode == 25) && (MShare.g_ItemArr[i].s.Name != "") && (MShare.g_ItemArr[i].s.Name.IndexOf("符") > 0))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    ret = 4;
                    result = true;
                    return result;
                }
                else
                {
                    has = false;
                    for (i = 0; i < MShare.MAXBAGITEMCL; i ++ )
                    {
                        if ((MShare.g_ItemArr[i].s.StdMode == 25) && (MShare.g_ItemArr[i].s.Name != "") && (MShare.g_ItemArr[i].s.Name.IndexOf("药") > 0))
                        {
                            has = true;
                            break;
                        }
                    }
                    if (!has)
                    {
                        ret = 5;
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }

        public static int GetDis(int X1, int Y1, int X2, int Y2)
        {
            int result;
            result = (X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2);
            return result;
        }

        public static bool IsProperTarget(TActor Actor)
        {
            bool result;
            // (Actor.m_nHiterCode = 0) and
            result = (Actor != null) && (Actor.m_btRace != 0) && (Actor.m_sUserName != "") && (!(new ArrayList(new int[] {12, 50}).Contains(Actor.m_btRace))) && (!Actor.m_boDeath) && (Actor.m_btRace != 12) && ((Actor.m_nState & Grobal2.STATE_STONE_MODE) == 0) && (Actor.m_sUserName.IndexOf("(") == 0) && (Actor.m_boVisible) && (!Actor.m_boDelActor) && (!Actor.m_btAFilter) && (MShare.g_gcAss[6] && (MShare.g_APMobList.IndexOf(Actor.m_sUserName) < 0));
            return result;
        }

        public static TActor SearchTarget()
        {
            TActor result;
            int i;
            TActor Actor;
            int dx;
            int distance;
            result = null;
            distance = 10000;
            if (MShare.g_APTagget != null)
            {
                if ((!MShare.g_APTagget.m_boDeath) && (MShare.g_APTagget.m_nHiterCode == MShare.g_MySelf.m_nRecogId) && (MShare.g_APTagget.m_boVisible) && (!MShare.g_APTagget.m_boDelActor))
                {
                    distance = GetDis(MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
                    result = MShare.g_APTagget;
                // Exit;
                }
            }
            PlayScene _wvar1 =ClMain.g_PlayScene;
            for (i = 0; i < _wvar1.MActorList.Count; i ++ )
            {
                Actor = ((TActor)(_wvar1.MActorList[i]));
                if (IsProperTarget(Actor))
                {
                    dx = GetDis(Actor.m_nCurrX, Actor.m_nCurrY, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
                    if (dx < distance)
                    {
                        distance = dx;
                        result = Actor;
                    }
                }
            }
            return result;
        }

        public static int GetDropItemsDis()
        {
            int result;
            int i;
            int dx;
            TDropItem d;
            result = 100000;
            for (i = 0; i < MShare.g_DropedItemList.Count; i ++ )
            {
                d = ((TDropItem)(MShare.g_DropedItemList[i]));
                if (MShare.g_boPickUpAll || d.boPickUp)
                {
                    // 如果拾取过滤，则判断是否过滤
                    dx = GetDis(d.X, d.Y, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
                    // 获取距离，选择最近的
                    if ((dx < result) && (dx != 0))
                    {
                        MShare.g_AutoPicupItem = d;
                        result = dx;
                    }
                }
            }
            return result;
        }

        public static int GetAutoPalyStation()
        {
            int result;
            bool has;
            bool bPcaketfull;
            int i;
            int Mobdistance;
            int ItemDistance;
            // 判断是否回城
            // if GetTickCount - g_APRunTick >= 200 then begin
            // g_APRunTick := GetTickCount;
            // end else
            // exit;
            // Result := 0;
            bPcaketfull = false;
            if (IsBackToSafeZone(ref Mobdistance))
            {
                result = 0;
                return result;
            }
            else
            {
                has = false;
                for (i = 0; i <= 45; i ++ )
                {
                    if ((MShare.g_ItemArr[i].s.Name == ""))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    // 包满
                    bPcaketfull = true;
                }
            }
            if (MShare.g_nOverAPZone > 0)
            {
                // Dec(g_nOverAPZone);
                result = 4;
                return result;
            }
            if ((MShare.g_APMapPath != null) && (MShare.g_APStep >= 0) && (MShare.g_APStep <= MShare.g_APMapPath.GetUpperBound(0)))
            {
                // 正在循路，超出范围。。。
                if (MShare.g_APLastPoint.X >= 0)
                {
                    if ((((Math.Abs(MShare.g_APLastPoint.X - MShare.g_MySelf.m_nCurrX) >= overdisc) || (Math.Abs(MShare.g_APLastPoint.X - MShare.g_MySelf.m_nCurrY) >= overdisc))) && ((Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrX) >= overdisc) || (Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrY) >= overdisc)))
                    {
                        MShare.g_nOverAPZone = 14;
                        result = 4;
                        return result;
                    }
                }
                else
                {
                    if (((Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrX) >= overdisc) || (Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrY) >= overdisc)))
                    {
                        MShare.g_nOverAPZone = 14;
                        result = 4;
                        return result;
                    }
                }
            }
            // 获取最近的怪物
            if ((MShare.g_APTagget != null))
            {
                if (MShare.g_APTagget.m_boDelActor || MShare.g_APTagget.m_boDeath)
                {
                    // or (abs(g_APTagget.m_nCurrX - g_MySelf.m_nCurrX) > 15) or (abs(g_APTagget.m_nCurrY - g_MySelf.m_nCurrY) > 15) then
                    MShare.g_APTagget = null;
                }
            }
            if (((MShare.GetTickCount() - MShare.g_dwSearchEnemyTick) > 4000) || (((MShare.GetTickCount() - MShare.g_dwSearchEnemyTick) > 300) && ((MShare.g_APTagget == null))))
            {
                MShare.g_dwSearchEnemyTick = MShare.GetTickCount();
                MShare.g_APTagget = SearchTarget();
            }
            if ((MShare.g_APTagget != null))
            {
                if (MShare.g_APTagget.m_boDelActor || MShare.g_APTagget.m_boDeath)
                {
                    // or (abs(g_APTagget.m_nCurrX - g_MySelf.m_nCurrX) > 15) or (abs(g_APTagget.m_nCurrY - g_MySelf.m_nCurrY) > 15) then
                    MShare.g_APTagget = null;
                }
            }
            if (MShare.g_APTagget != null)
            {
                // GetDistance
                Mobdistance = GetDis(MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
            }
            else
            {
                Mobdistance = 100000;
            }
            ItemDistance = 0;
            // 获取最近的物品
            if (!bPcaketfull)
            {
                ItemDistance = GetDropItemsDis();
            }
            else
            {
                MShare.g_AutoPicupItem = null;
            }
            // 两者都没有发现
            if ((ItemDistance == 100000) && ((Mobdistance == 100000) || (Mobdistance == 0)))
            {
                // 没有发现怪物或物品，随机走
                result = 3;
                return result;
            }
            if ((ItemDistance + 2) >= Mobdistance)
            {
                // 优先杀怪
                // 发现怪物
                result = 1;
            }
            else
            {
                result = 2;
            }
            // 发现物品

            return result;
        }

        public static bool AutoUseMagic(byte Key, TActor target, int nx, int ny)
        {
            bool result;
            TClientMagic pcm;
            result = true;
            pcm =ClMain.frmMain.GetMagicByID(Key);
            if (pcm == null)
            {
                result = false;
                return result;
            }
            MShare.g_FocusCret = target;
            if (nx >= 0)
            {
               ClMain.frmMain.UseMagic(nx, ny, pcm, true);
            }
            else
            {
               ClMain.frmMain.UseMagic(target.m_nCurrX, target.m_nCurrY, pcm);
            }
            return result;
        }

        public static int TargetCount(TActor target)
        {
            int result;
            int rx;
            int ry;
            TActor Actor;
            result = 1;
            rx = target.m_nCurrX + 1;
            ry = target.m_nCurrY;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            rx = target.m_nCurrX + 1;
            ry = target.m_nCurrY + 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX + 1;
            ry = target.m_nCurrY - 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX - 1;
            ry = target.m_nCurrY;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX - 1;
            ry = target.m_nCurrY + 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX - 1;
            ry = target.m_nCurrY - 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX;
            ry = target.m_nCurrY + 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;
            rx = target.m_nCurrX;
            ry = target.m_nCurrY - 1;
            Actor =ClMain.g_PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(Actor))
            {
                result ++;
            }
            // if Result > 2 then Exit;

            return result;
        }

        public static int TargetCount2(TActor target)
        {
            int result;
            int i;
            TActor Actor;
            result = 0;
            PlayScene _wvar1 =ClMain.g_PlayScene;
            for (i = 0; i < _wvar1.MActorList.Count; i ++ )
            {
                Actor = ((TActor)(_wvar1.MActorList[i]));
                if ((Math.Abs(Actor.m_nCurrX - MShare.g_MySelf.m_nCurrX) < 6) || (Math.Abs(Actor.m_nCurrY - MShare.g_MySelf.m_nCurrY) < 6))
                {
                    if (IsProperTarget(Actor))
                    {
                        result ++;
                    }
                }
            }
            return result;
        }

        public static int TargetCount3(TActor target)
        {
            int result;
            int i;
            TActor Actor;
            result = 0;
            PlayScene _wvar1 =ClMain.g_PlayScene;
            for (i = 0; i < _wvar1.MActorList.Count; i ++ )
            {
                Actor = ((TActor)(_wvar1.MActorList[i]));
                if ((Math.Abs(Actor.m_nCurrX - MShare.g_MySelf.m_nCurrX) < 5) || (Math.Abs(Actor.m_nCurrY - MShare.g_MySelf.m_nCurrY) < 5))
                {
                    if (IsProperTarget(Actor))
                    {
                        result ++;
                    }
                }
            }
            return result;
        }

        public static int TargetHumCount(TActor target)
        {
            int result;
            bool b;
            int i;
            TActor Actor;
            result = 0;
            PlayScene _wvar1 =ClMain.g_PlayScene;
            for (i = 0; i < _wvar1.MActorList.Count; i ++ )
            {
                Actor = ((TActor)(_wvar1.MActorList[i]));
                if ((Math.Abs(Actor.m_nCurrX - MShare.g_MySelf.m_nCurrX) < 8) || (Math.Abs(Actor.m_nCurrY - MShare.g_MySelf.m_nCurrY) < 8))
                {
                    b = (Actor != null) && (!Actor.m_boDeath) && ((Actor.m_btRace == 0) || (Actor.m_btIsHero == 1));
                    if (b)
                    {
                        result ++;
                    }
                }
            }
            return result;
        }

        public static bool XPATTACK()
        {
            bool result;
            TClientMagic pcm;
            result = false;
            if (MShare.g_MySelf.m_HeroObject != null)
            {
                if ((MShare.g_MySelf.m_HeroObject.m_nHeroEnergyType != 2) && (MShare.g_MySelf.m_HeroObject.m_nHeroEnergy > 0) && (MShare.g_MySelf.m_HeroObject.m_nHeroEnergy >= MShare.g_MySelf.m_HeroObject.m_nMaxHeroEnergy))
                {
                    if (MShare.GetTickCount() - MShare.g_dwLatestJoinAttackTick <= 3000)
                    {
                        return result;
                    }
                    pcm = null;
                    switch(MShare.g_MySelf.m_btJob)
                    {
                        case 0:
                            switch(MShare.g_MySelf.m_HeroObject.m_btJob)
                            {
                                case 0:
                                    pcm =ClMain.frmMain.HeroGetMagicByID(60);
                                    break;
                                case 1:
                                    pcm =ClMain.frmMain.HeroGetMagicByID(62);
                                    break;
                                case 2:
                                    pcm =ClMain.frmMain.HeroGetMagicByID(61);
                                    break;
                            }
                            if ((pcm != null) &&ClMain.frmMain.CanNextAction() &&ClMain.frmMain.ServerAcceptNextAction() &&ClMain.frmMain.CanNextHit())
                            {
                                MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                                MShare.g_dwLastMoveTick = MShare.GetTickCount();
                                result = true;
                                MShare.g_FocusCret = MShare.g_APTagget;
                               ClMain.frmMain.SendHeroSetTarget();
                               ClMain.frmMain.SendHeroJoinAttack();
                                MShare.g_dwLatestJoinAttackTick = MShare.GetTickCount();
                                return result;
                            }
                            break;
                        case 1:
                            if (CanNextSpell())
                            {
                                switch(MShare.g_MySelf.m_HeroObject.m_btJob)
                                {
                                    case 0:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(62);
                                        break;
                                    case 1:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(65);
                                        break;
                                    case 2:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(64);
                                        break;
                                }
                            }
                            break;
                        case 2:
                            if (CanNextSpell())
                            {
                                switch(MShare.g_MySelf.m_HeroObject.m_btJob)
                                {
                                    case 0:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(61);
                                        break;
                                    case 1:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(64);
                                        break;
                                    case 2:
                                        pcm =ClMain.frmMain.HeroGetMagicByID(63);
                                        break;
                                }
                            }
                            break;
                    }
                    if ((pcm != null) &&ClMain.frmMain.CanNextAction() &&ClMain.frmMain.ServerAcceptNextAction())
                    {
                        MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                        MShare.g_dwLastMoveTick = MShare.GetTickCount();
                        result = true;
                        MShare.g_FocusCret = MShare.g_APTagget;
                       ClMain.frmMain.SendHeroSetTarget();
                       ClMain.frmMain.SendHeroJoinAttack();
                        MShare.g_dwLatestJoinAttackTick = MShare.GetTickCount();
                    }
                }
            }
            return result;
        }

        public static bool HeroAttackTagget(TActor target)
        {
            bool result;
            int n;
            int m;
            int tdir;
            int MagicKey;
            TClientMagic pcm;
            int i;
            int nTag;
            int nx;
            int ny;
            int nAbsX;
            int nAbsY;
            int nNX;
            int nNY;
            int nTX;
            int nTY;
            int nOldDC;
            result = false;
            MShare.g_boAPAutoMove = false;
            MShare.g_nTagCount = 0;
            if ((MShare.g_MySelf == null) || MShare.g_MySelf.m_boDeath || (MShare.g_APTagget == null) || MShare.g_APTagget.m_boDeath)
            {
                return result;
            }
            switch(MShare.g_MySelf.m_btJob)
            {
                case 0:
                    if (MShare.g_SeriesSkillReady)
                    {
                       ClMain.frmMain.SendFireSerieSkill();
                    // Result := True;
                    }
                    if (MShare.g_gcTec[4] && (MShare.g_MySelf.m_nState & 0x00100000 == 0) && CanNextSpell())
                    {
                        if (MShare.g_MagicArr[0][31] != null)
                        {
                           ClMain.frmMain.UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, MShare.g_MagicArr[0][31]);
                            return result;
                        }
                    }
                    if (XPATTACK() ||ClMain.frmMain.AttackTarget(MShare.g_APTagget))
                    {
                        result = true;
                        return result;
                    }
                    break;
                case 1:
                    if (MShare.g_MySelf.m_Abil.Level < 7)
                    {
                        if (ClMain.frmMain.AttackTarget(MShare.g_APTagget))
                        {
                            result = true;
                        }
                        return result;
                    }
                    if (MShare.g_gcTec[4] && (MShare.g_MySelf.m_nState & 0x00100000 == 0) && CanNextSpell())
                    {
                        if (MShare.g_MagicArr[0][31] != null)
                        {
                           ClMain.frmMain.UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, MShare.g_MagicArr[0][31]);
                            return result;
                        }
                    }
                    if (MShare.g_SeriesSkillReady && (MShare.g_MagicLockActor != null) && (!MShare.g_MagicLockActor.m_boDeath))
                    {
                       ClMain.frmMain.SendFireSerieSkill();
                    }
                    if (XPATTACK())
                    {
                        result = true;
                        return result;
                    }
                    MagicKey = 11;
                    nAbsX = Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_APTagget.m_nCurrX);
                    nAbsY = Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_APTagget.m_nCurrY);
                    if (((nAbsX > 2) || (nAbsY > 2)))
                    {
                        if ((nAbsX <= MShare.g_nMagicRange) && (nAbsY <= MShare.g_nMagicRange))
                        {
                            result = true;
                            //@ Unsupported function or procedure: 'Format'
                            MShare.g_sAPStr = Format("[挂机] 怪物目标：%s (%d,%d) 正在使用魔法攻击", new int[] {MShare.g_APTagget.m_sUserName, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY});
                            if (ClMain.frmMain.CanNextAction() &&ClMain.frmMain.ServerAcceptNextAction())
                            {
                                if (CanNextSpell())
                                {
                                    if ((MShare.g_MagicArr[0][22] != null))
                                    {
                                        if (TargetCount3(MShare.g_APTagget) >= 15)
                                        {
                                            tdir = ClFunc.GetNextDirection(MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
                                            ClFunc.GetFrontPosition(MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY, tdir, ref nx, ref ny);
                                            ClFunc.GetFrontPosition(nx, ny, tdir, ref nx, ref ny);
                                            if (ClMain.EventMan.GetEvent(nx, ny, Grobal2.ET_FIRE) == null)
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                if (AutoUseMagic(22, MShare.g_APTagget, nx, ny))
                                                {
                                                    return result;
                                                }
                                                else
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                            }
                                        }
                                    }
                                    nOldDC = 3;
FFFF:
                                    if ((MShare.g_MagicArr[0][10] != null))
                                    {
                                        tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                                        if (ClMain.GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, 1, ref nNX, ref nNY))
                                        {
                                           ClMain.GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, 8, ref nTX, ref nTY);
                                            if (ClMain.CheckMagPassThrough(nNX, nNY, nTX, nTY, tdir) >= nOldDC)
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                MagicKey = 10;
                                                goto AAAA; //@ Unsupport goto 
                                            }
                                        }
                                    }
                                    if ((MShare.g_MagicArr[0][9] != null))
                                    {
                                        tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                                        if (ClMain.GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, 1, ref nNX, ref nNY))
                                        {
                                           ClMain.GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, 5, ref nTX, ref nTY);
                                            if (ClMain.CheckMagPassThrough(nNX, nNY, nTX, nTY, tdir) >= nOldDC)
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                MagicKey = 9;
                                                goto AAAA; //@ Unsupport goto 
                                            }
                                        }
                                    }
                                    if (MShare.m_btMagPassTh > 0)
                                    {
                                        MShare.m_btMagPassTh -= 1;
                                        nOldDC = 1;
                                        goto FFFF; //@ Unsupport goto 
                                    }
                                    if (MShare.g_MagicArr[0][11] != null)
                                    {
                                        MagicKey = 11;
                                    }
                                    else if (MShare.g_MagicArr[0][5] != null)
                                    {
                                        MagicKey = 5;
                                    }
                                    else if (MShare.g_MagicArr[0][1] != null)
                                    {
                                        MagicKey = 1;
                                    }
                                    MShare.g_nTagCount = TargetCount(MShare.g_APTagget);
                                    if (MShare.g_nTagCount >= 2)
                                    {
                                        if ((new System.Random(7)).Next() > 1)
                                        {
                                            if ((MShare.g_MagicArr[0][58] != null) && ((new System.Random(8)).Next() > 1))
                                            {
                                                MagicKey = 58;
                                            }
                                            else if ((MShare.g_MagicArr[0][33] != null))
                                            {
                                                MagicKey = 33;
                                            }
                                        }
                                        else if ((MShare.g_MagicArr[0][47] != null))
                                        {
                                            MagicKey = 47;
                                        }
                                        if ((MagicKey <= 11) && (MShare.g_MagicArr[0][23] != null))
                                        {
                                            MagicKey = 23;
                                        }
                                    }
AAAA:
                                    if (AutoUseMagic(MagicKey, MShare.g_APTagget))
                                    {
                                        return result;
                                    }
                                    else
                                    {
                                        result = false;
                                        return result;
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = false;
                            return result;
                        }
                    }
                    else
                    {
                        if (((nAbsX <= 1) && (nAbsY <= 1)))
                        {
                            // 目标近身
                            if (CanNextSpell())
                            {
                                nTag = TargetCount(MShare.g_MySelf);
                                if ((nTag >= 5))
                                {
                                    // 怪太多,强攻解围...
DDDD:
                                    if (CanNextSpell())
                                    {
                                        MagicKey = 0;
                                        if ((new System.Random(7)).Next() > 1)
                                        {
                                            if ((MShare.g_MagicArr[0][58] != null) && ((new System.Random(8)).Next() > 1))
                                            {
                                                MagicKey = 58;
                                            }
                                            else if ((MShare.g_MagicArr[0][33] != null))
                                            {
                                                MagicKey = 33;
                                            }
                                        }
                                        else if ((MShare.g_MagicArr[0][47] != null))
                                        {
                                            MagicKey = 47;
                                        }
                                        if ((MagicKey <= 11) && (MShare.g_MagicArr[0][23] != null))
                                        {
                                            MagicKey = 23;
                                        }
                                        if (MagicKey > 0)
                                        {
                                            result = true;
                                            goto AAAA; //@ Unsupport goto 
                                        }
                                        if (MShare.g_MagicArr[0][11] != null)
                                        {
                                            MagicKey = 11;
                                        }
                                        else if (MShare.g_MagicArr[0][5] != null)
                                        {
                                            MagicKey = 5;
                                        }
                                        else if (MShare.g_MagicArr[0][1] != null)
                                        {
                                            MagicKey = 1;
                                        }
                                        if (MagicKey > 0)
                                        {
                                            result = true;
                                            goto AAAA; //@ Unsupport goto 
                                        }
                                    }
                                    result = false;
                                    return result;
                                // 躲避
                                }
                                if ((nTag >= 4) && (MShare.g_MagicArr[0][8] != null))
                                {
                                    // 比较勉强的抗拒...一般选择逃避
                                    result = true;
                                    MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                    if (AutoUseMagic(8, MShare.g_MySelf))
                                    {
                                        if (MShare.m_btMagPassTh <= 0)
                                        {
                                            MShare.m_btMagPassTh += 1 + (new System.Random(2)).Next();
                                        }
                                        return result;
                                    }
                                }
                            }
                        }
                        tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                        // 避怪
                        ClFunc.GetBackPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nx, ref ny);
                        nTag = 0;
                        while (true)
                        {
                            if (ClMain.g_PlayScene.CanWalk(nx, ny))
                            {
                                break;
                            }
                            tdir ++;
                            tdir = tdir % 8;
                            ClFunc.GetBackPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nx, ref ny);
                            nTag ++;
                            if (nTag > 8)
                            {
                                break;
                            }
                        }
                        if (ClMain.g_PlayScene.CanWalk(nx, ny))
                        {
                            ClFunc.GetBackPosition2(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nTX, ref nTY);
                            // Map.CanMove(nTX, nTY)
                            if (ClMain.g_PlayScene.CanWalk(nTX, nTY))
                            {
                                // DScreen.AddChatBoardString(Format('避怪2(%d:%d)...........', [nTX, nTY]), clBlue, clWhite);
                                MShare.g_nTargetX = nTX;
                                MShare.g_nTargetY = nTY;
                                MShare.g_ChrAction = MShare.TChrAction.caRun;
                                MShare.g_nMouseCurrX = nTX;
                                MShare.g_nMouseCurrY = nTY;
                                result = true;
                            }
                            else
                            {
                                // DScreen.AddChatBoardString(Format('避怪(%d:%d)...........', [nX, nY]), clBlue, clWhite);
                                MShare.g_nTargetX = nx;
                                MShare.g_nTargetY = ny;
                                MShare.g_ChrAction = MShare.TChrAction.caRun;
                                MShare.g_nMouseCurrX = nx;
                                MShare.g_nMouseCurrY = ny;
                                result = true;
                            }
                        }
                        else
                        {
                            // 强攻
                            // DScreen.AddChatBoardString('强攻...........', clBlue, clWhite);
                            // nTag := 4;
                            goto DDDD; //@ Unsupport goto 
                        }
                    }
                    break;
                case 2:
                    // //////////////////////////////////////
                    if (MShare.g_gcTec[4] && (MShare.g_MySelf.m_nState & 0x00100000 == 0) && CanNextSpell())
                    {
                        if (MShare.g_MagicArr[0][31] != null)
                        {
                           ClMain.frmMain.UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, MShare.g_MagicArr[0][31]);
                            return result;
                        }
                    }
                    n = 0;
                    if ((MShare.g_UseItems[Grobal2.U_ARMRINGL].s.StdMode == 25) && (MShare.g_UseItems[Grobal2.U_ARMRINGL].s.Shape != 6) && (MShare.g_UseItems[Grobal2.U_ARMRINGL].s.Name.IndexOf("药") > 0))
                    {
                        n ++;
                    }
                    if (n == 0)
                    {
                        for (i = 6; i < MShare.MAXBAGITEMCL; i ++ )
                        {
                            if ((MShare.g_ItemArr[i].s.NeedIdentify < 4) && (MShare.g_ItemArr[i].s.StdMode == 25) && (MShare.g_ItemArr[i].s.Shape != 6) && (MShare.g_ItemArr[i].s.Name.IndexOf("药") > 0))
                            {
                                n ++;
                                break;
                            }
                        }
                    }
                    if (n == 0)
                    {
                        if (MShare.GetTickCount() - g_hinttick1 > 60 * 1000)
                        {
                            g_hinttick1 = MShare.GetTickCount();
                           ClMain.DScreen.AddChatBoardString("你的[药粉]已经用完，注意补充", System.Drawing.Color.White, System.Drawing.Color.Blue);
                        }
                    }
                    m = 0;
                    if ((MShare.g_UseItems[Grobal2.U_ARMRINGL].s.StdMode == 25) && (MShare.g_UseItems[Grobal2.U_ARMRINGL].s.Shape != 6) && (MShare.g_UseItems[Grobal2.U_ARMRINGL].s.Name.IndexOf("符") > 0))
                    {
                        m ++;
                    }
                    if (m == 0)
                    {
                        for (i = 6; i < MShare.MAXBAGITEMCL; i ++ )
                        {
                            if ((MShare.g_ItemArr[i].s.NeedIdentify < 4) && (MShare.g_ItemArr[i].s.StdMode == 25) && (MShare.g_ItemArr[i].s.Shape != 6) && (MShare.g_ItemArr[i].s.Name.IndexOf("符") > 0))
                            {
                                m ++;
                                break;
                            }
                        }
                    }
                    if (m == 0)
                    {
                        if (MShare.GetTickCount() - g_hinttick2 > 60 * 1000)
                        {
                            g_hinttick2 = MShare.GetTickCount();
                           ClMain.DScreen.AddChatBoardString("你的[护身符]已经用完，注意补充", System.Drawing.Color.White, System.Drawing.Color.Blue);
                        }
                    }
                    if ((MShare.GetTickCount() - MShare.m_dwRecallTick) > (1000 * 6))
                    {
                        // 设置比较大时间,以便其他攻击...
                        MShare.m_dwRecallTick = MShare.GetTickCount();
                        if ((MShare.g_MySelf.m_SlaveObject.Count == 0) && (m > 0))
                        {
                            MagicKey = 0;
                            if ((MShare.g_MagicArr[0][55] != null))
                            {
                                MagicKey = 55;
                            }
                            else if ((MShare.g_MagicArr[0][30] != null))
                            {
                                MagicKey = 30;
                            }
                            else if ((MShare.g_MagicArr[0][17] != null))
                            {
                                MagicKey = 17;
                            }
                            if ((MagicKey != 0))
                            {
                                result = true;
                                pcm =ClMain.frmMain.GetMagicByID(MagicKey);
                                if (pcm == null)
                                {
                                    result = false;
                                    return result;
                                }
                                MShare.g_FocusCret = null;
                                tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                                ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nx, ref ny);
                               ClMain.frmMain.UseMagic(nx, ny, pcm, true);
                                return result;
                            }
                        }
                    }
                    if ((MShare.GetTickCount() - MShare.m_dwSpellTick) > (1000 * 5))
                    {
                        // 状态类魔法...
                        MShare.m_dwSpellTick = MShare.GetTickCount();
                        // MAGDEFENCEUP
                        if ((MShare.g_MagicArr[0][14] != null) && (m > 0))
                        {
                            if ((MShare.g_MySelf.m_nState & 0x00200000 == 0))
                            {
                                result = true;
                                if (AutoUseMagic(14, MShare.g_MySelf))
                                {
                                    return result;
                                }
                            }
                            if (MShare.g_MySelf.m_HeroObject != null)
                            {
                                if ((MShare.g_MySelf.m_HeroObject.m_nState & 0x00200000) == 0)
                                {
                                    result = true;
                                    if (AutoUseMagic(14, MShare.g_MySelf.m_HeroObject))
                                    {
                                        return result;
                                    }
                                }
                            }
                        }
                        // Double DefenceUp
                        if ((MShare.g_MagicArr[0][15] != null) && (m > 0))
                        {
                            if ((MShare.g_MySelf.m_nState & 0x00400000) == 0)
                            {
                                result = true;
                                if (AutoUseMagic(15, MShare.g_MySelf))
                                {
                                    return result;
                                }
                            }
                            if (MShare.g_MySelf.m_HeroObject != null)
                            {
                                if ((MShare.g_MySelf.m_HeroObject.m_nState & 0x00400000 == 0))
                                {
                                    result = true;
                                    if (AutoUseMagic(15, MShare.g_MySelf.m_HeroObject))
                                    {
                                        return result;
                                    }
                                }
                            }
                        }
                        // Healling
                        if ((MShare.g_MagicArr[0][2] != null))
                        {
                            if ((Math.Round((MShare.g_MySelf.m_Abil.HP / MShare.g_MySelf.m_Abil.MaxHP) * 100) < 85))
                            {
                                result = true;
                                if (AutoUseMagic(2, MShare.g_MySelf))
                                {
                                    return result;
                                }
                            }
                            if (MShare.g_MySelf.m_HeroObject != null)
                            {
                                if ((MShare.g_MySelf.m_HeroObject.m_Abil.HP != 0) && (Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_MySelf.m_HeroObject.m_nCurrX + 2) <= MShare.g_nMagicRange) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_MySelf.m_HeroObject.m_nCurrY + 2) <= MShare.g_nMagicRange))
                                {
                                    if ((Math.Round((MShare.g_MySelf.m_HeroObject.m_Abil.HP / MShare.g_MySelf.m_HeroObject.m_Abil.MaxHP) * 100) <= 80))
                                    {
                                        result = true;
                                        if (AutoUseMagic(2, MShare.g_MySelf.m_HeroObject))
                                        {
                                            return result;
                                        }
                                    }
                                }
                            }
                            if (MShare.g_MySelf.m_SlaveObject.Count > 0)
                            {
                                for (i = 0; i < MShare.g_MySelf.m_SlaveObject.Count; i ++ )
                                {
                                    if (((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_boDeath)
                                    {
                                        continue;
                                    }
                                    if ((((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.HP != 0) && (Math.Abs(MShare.g_MySelf.m_nCurrX - ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_nCurrX + 2) <= MShare.g_nMagicRange) && (Math.Abs(MShare.g_MySelf.m_nCurrY - ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_nCurrY + 2) <= MShare.g_nMagicRange))
                                    {
                                        if ((Math.Round((((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.HP / ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.MaxHP) * 100) <= 80))
                                        {
                                            result = true;
                                            if (AutoUseMagic(2, ((TActor)(MShare.g_MySelf.m_SlaveObject[i]))))
                                            {
                                                return result;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((MShare.g_MySelf.m_Abil.Level < 18) || (MShare.g_MagicArr[0][13] == null) || ((n == 0) && (m == 0)))
                    {
CCCC:
                        if ((MShare.GetTickCount() - MShare.m_dwSpellTick) > 3000)
                        {
                            MShare.m_dwSpellTick = MShare.GetTickCount();
                            if ((MShare.g_MagicArr[0][2] != null))
                            {
                                result = true;
                                if ((Math.Round((MShare.g_MySelf.m_Abil.HP / MShare.g_MySelf.m_Abil.MaxHP) * 100) < 85))
                                {
                                    if (AutoUseMagic(2, MShare.g_MySelf))
                                    {
                                        return result;
                                    }
                                }
                                if (MShare.g_MySelf.m_HeroObject != null)
                                {
                                    if ((MShare.g_MySelf.m_HeroObject.m_Abil.HP != 0) && (Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_MySelf.m_HeroObject.m_nCurrX + 2) <= MShare.g_nMagicRange) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_MySelf.m_HeroObject.m_nCurrY + 2) <= MShare.g_nMagicRange))
                                    {
                                        if ((Math.Round((MShare.g_MySelf.m_HeroObject.m_Abil.HP / MShare.g_MySelf.m_HeroObject.m_Abil.MaxHP) * 100) < 85))
                                        {
                                            if (AutoUseMagic(2, MShare.g_MySelf.m_HeroObject))
                                            {
                                                return result;
                                            }
                                        }
                                    }
                                }
                                for (i = 0; i < MShare.g_MySelf.m_SlaveObject.Count; i ++ )
                                {
                                    if (((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_boDeath)
                                    {
                                        continue;
                                    }
                                    if ((Math.Abs(MShare.g_MySelf.m_nCurrX - ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_nCurrX + 2) <= MShare.g_nMagicRange) && (Math.Abs(MShare.g_MySelf.m_nCurrY - ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_nCurrY + 2) <= MShare.g_nMagicRange))
                                    {
                                        if ((((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.HP != 0) && (Math.Round((((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.HP / ((TActor)(MShare.g_MySelf.m_SlaveObject[i])).m_Abil.MaxHP) * 100) < 85))
                                        {
                                            if (AutoUseMagic(2, ((TActor)(MShare.g_MySelf.m_SlaveObject[i]))))
                                            {
                                                return result;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ClMain.frmMain.AttackTarget(MShare.g_APTagget))
                        {
                            result = true;
                        }
                        return result;
                    }
                    if (MShare.g_SeriesSkillReady && (MShare.g_MagicLockActor != null) && (!MShare.g_MagicLockActor.m_boDeath))
                    {
                       ClMain.frmMain.SendFireSerieSkill();
                    }
                    if (XPATTACK())
                    {
                        result = true;
                        return result;
                    }
                    MagicKey = 0;
                    nAbsX = Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_APTagget.m_nCurrX);
                    nAbsY = Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_APTagget.m_nCurrY);
                    if (((nAbsX > 2) || (nAbsY > 2)))
                    {
                        // 需要快速检测类...
                        if ((nAbsX <= MShare.g_nMagicRange) && (nAbsY <= MShare.g_nMagicRange))
                        {
                            result = true;
                            //@ Unsupported function or procedure: 'Format'
                            MShare.g_sAPStr = Format("[挂机] 怪物目标：%s (%d,%d) 正在使用魔法攻击", new int[] {MShare.g_APTagget.m_sUserName, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY});
                            if (ClMain.frmMain.CanNextAction() &&ClMain.frmMain.ServerAcceptNextAction())
                            {
EEEE:
                                if (CanNextSpell())
                                {
                                    // DoubluSC
                                    if ((MShare.g_MagicArr[0][50] != null))
                                    {
                                        if (MShare.GetTickCount() - MShare.m_dwDoubluSCTick > 90 * 1000)
                                        {
                                            MShare.m_dwDoubluSCTick = MShare.GetTickCount();
                                            MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                            MagicKey = 50;
                                            goto BBBB; //@ Unsupport goto 
                                        }
                                    }
                                    // DECHEALTH & DAMAGEARMOR
                                    if (MShare.GetTickCount() - MShare.m_dwPoisonTick > 3500)
                                    {
                                        MShare.m_dwPoisonTick = MShare.GetTickCount();
                                        if ((MShare.g_MagicArr[0][6] != null))
                                        {
                                            if ((MShare.g_APTagget.m_nState & 0x80000000 == 0) || (MShare.g_APTagget.m_nState & 0x40000000 == 0))
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                MagicKey = 6;
                                                goto BBBB; //@ Unsupport goto 
                                            }
                                        }
                                        if ((MShare.g_MagicArr[0][18] != null) && (MShare.g_MySelf.m_nState & 0x00800000 == 0) && ((new System.Random(4)).Next() == 0))
                                        {
                                            if ((TargetCount2(MShare.g_MySelf) >= 7))
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                MagicKey = 18;
                                                goto BBBB; //@ Unsupport goto 
                                            }
                                        }
                                    }
                                    if ((MShare.g_MagicArr[0][13] != null) || (MShare.g_MagicArr[0][57] != null))
                                    {
                                        if ((MShare.g_MagicArr[0][57] != null) && (((Math.Round((MShare.g_MySelf.m_Abil.HP / MShare.g_MySelf.m_Abil.MaxHP) * 100) < 80) && ((new System.Random(100 - Math.Round((MShare.g_MySelf.m_Abil.HP / MShare.g_MySelf.m_Abil.MaxHP) * 100))).Next() > 5)) || ((new System.Random(10)).Next() > 6)))
                                        {
                                            MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                            MagicKey = 57;
                                            goto BBBB; //@ Unsupport goto 
                                        }
                                        if ((MShare.g_MagicArr[0][13] != null))
                                        {
                                            MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                            MagicKey = 13;
                                            goto BBBB; //@ Unsupport goto 
                                        }
                                    }
BBBB:
                                    if (MagicKey > 0)
                                    {
                                        if (AutoUseMagic(MagicKey, MShare.g_APTagget))
                                        {
                                            return result;
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        goto CCCC; //@ Unsupport goto 
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = false;
                            return result;
                        }
                    }
                    else
                    {
                        if (((nAbsX <= 1) && (nAbsY <= 1)))
                        {
                            // 目标近身
                            if (CanNextSpell())
                            {
                                nTag = TargetCount(MShare.g_MySelf);
                                if ((nTag >= 5))
                                {
                                    // 怪太多,强攻解围...
                                    goto EEEE; //@ Unsupport goto 
                                }
                                if ((MShare.g_MagicArr[0][48] != null) && (nTag >= 3))
                                {
                                    // 有力的抗拒...不同于法师逃避
                                    result = true;
                                    MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                    if (AutoUseMagic(48, MShare.g_MySelf))
                                    {
                                        if (MShare.m_btMagPassTh <= 0)
                                        {
                                            MShare.m_btMagPassTh += 1 + (new System.Random(2)).Next();
                                        }
                                        return result;
                                    }
                                }
                            }
                        }
                        tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                        // 避怪
                        ClFunc.GetBackPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nx, ref ny);
                        nTag = 0;
                        while (true)
                        {
                            if (ClMain.g_PlayScene.CanWalk(nx, ny))
                            {
                                break;
                            }
                            tdir ++;
                            tdir = tdir % 8;
                            ClFunc.GetBackPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nx, ref ny);
                            nTag ++;
                            if (nTag > 8)
                            {
                                break;
                            }
                        }
                        if (ClMain.g_PlayScene.CanWalk(nx, ny))
                        {
                            ClFunc.GetBackPosition2(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, tdir, ref nTX, ref nTY);
                            // Map.CanMove(nTX, nTY)
                            if (ClMain.g_PlayScene.CanWalk(nTX, nTY))
                            {
                                // DScreen.AddChatBoardString(Format('避怪2(%d:%d)...........', [nTX, nTY]), clBlue, clWhite);
                                MShare.g_nTargetX = nTX;
                                MShare.g_nTargetY = nTY;
                                MShare.g_ChrAction = MShare.TChrAction.caRun;
                                MShare.g_nMouseCurrX = nTX;
                                MShare.g_nMouseCurrY = nTY;
                                result = true;
                            }
                            else
                            {
                                // DScreen.AddChatBoardString(Format('避怪(%d:%d)...........', [nX, nY]), clBlue, clWhite);
                                MShare.g_nTargetX = nx;
                                MShare.g_nTargetY = ny;
                                MShare.g_ChrAction = MShare.TChrAction.caRun;
                                MShare.g_nMouseCurrX = nx;
                                MShare.g_nMouseCurrY = ny;
                                result = true;
                            }
                        }
                        else
                        {
                            // 强攻
                            // DScreen.AddChatBoardString('强攻...........', clBlue, clWhite);
                            goto EEEE; //@ Unsupport goto 
                        }
                    }
                    break;
            }
            return result;
        }

    } // end HeroActor

}

