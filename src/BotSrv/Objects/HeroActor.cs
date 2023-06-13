using BotSrv.Player;
using BotSrv.Scenes;
using System;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Enums;

namespace BotSrv.Objects
{
    public class HeroActor
    {
        private const int Overdisc = 22;
        private readonly RobotPlayer _robotClient;
        private long _gHinttick1;
        private long _gHinttick2;

        public HeroActor(RobotPlayer robotClient)
        {
            this._robotClient = robotClient;
        }

        private bool CanNextSpell()
        {
            var result = false;
            if (MShare.SpeedRate)
            {
                if (MShare.GetTickCount() - MShare.LatestSpellTick > MShare.SpellTime + MShare.g_dwMagicDelayTime - MShare.g_MagSpeedRate * 20)
                {
                    result = true;
                }
            }
            else
            {
                if (MShare.GetTickCount() - MShare.LatestSpellTick > MShare.SpellTime + MShare.g_dwMagicDelayTime)
                {
                    result = true;
                }
            }
            return result;
        }

        private void Enterqueue(MapTree node, int f)
        {
            var p = MShare.g_APQueue;
            var father = p;
            while (f > p.F)
            {
                father = p;
                p = p.Next;
                if (p == null) break;
            }
            var q = new MapLink();
            q.F = f;
            q.Node = node;
            q.Next = p;
            father.Next = q;
        }

        /// <summary>
        /// 将离目的地估计最近的方案出队列
        /// </summary>
        /// <returns></returns>
        private MapTree Dequeue()
        {
            var bestchoice = MShare.g_APQueue.Next.Node;
            MShare.g_APQueue.Next = MShare.g_APQueue.Next.Next;
            Dispose(MShare.g_APQueue.Next);
            return bestchoice;
        }

        /// <summary>
        /// 释放申请过的所有节点
        /// </summary>
        private void FreeTree()
        {
            while (MShare.g_APQueue != null)
            {
                var p = MShare.g_APQueue;
                if (p.Node != null) Dispose(p.Node);
                p.Node = null;
                MShare.g_APQueue = MShare.g_APQueue.Next;
                Dispose(p);
            }
        }

        // 估价函数,估价 x,y 到目的地的距离,估计值必须保证比实际值小
        private int Judge(int x, int y, int endX, int endY)
        {
            return Math.Abs(endX - x) + Math.Abs(endY - y);
        }

        private bool TryTileHas(short x, short y, int h)
        {
            var cx = x - _robotClient.Map.m_nBlockLeft;
            var cy = y - _robotClient.Map.m_nBlockTop;
            if (cx > BotConst.MAXX * 3 || cy > BotConst.MAXY * 3) return true;
            if (cx < 0 || cy < 0) return true;
            if (h < MShare.g_APPass[cx, cy])
            {
                return false;
            }
            return true;
        }

        private void Trytile(short x, short y, short endX, short endY, MapTree father, byte dir)
        {
            if (!_robotClient.Map.CanMove(x, y))
            {
                return;
            }
            MapTree p = father;
            while (p != null)
            {
                if (x == p.X && y == p.Y)
                {
                    return; // 如果 (x,y) 曾经经过,失败
                }
                p = p.Father;
            }
            var h = (ushort)(father.H + 1);
            if (TryTileHas(x, y, h))// 如果曾经有更好的方案移动到 (x,y) 失败
            {
                return;
            }
            MShare.g_APPass[x - _robotClient.Map.m_nBlockLeft, y - _robotClient.Map.m_nBlockTop] = h;// 记录这次到 (x,y) 的距离为历史最佳距离
            p = new MapTree();
            p.Father = father;
            p.H = father.H + 1;
            p.X = x;
            p.Y = y;
            p.Dir = dir;
            Enterqueue(p, p.H + Judge(x, y, endX, endY));
        }

        /// <summary>
        /// 路径寻找
        /// </summary>
        public void AutoFindPath(short startx, short starty, short endX, short endY)
        {
            if (!_robotClient.Map.CanMove(endX, endY))
            {
                return;
            }
            MShare.g_APPass = (ushort[,])MShare.g_APPassEmpty.Clone();
            InitQueue();
            MapTree root = new MapTree();
            root.X = startx;
            root.Y = starty;
            root.H = 0;
            root.Father = null;
            Enterqueue(root, Judge(startx, starty, endX, endY));
            var tryCount = 0;
            while (true)
            {
                root = Dequeue();
                if (root == null) break;
                var x = root.X;
                var y = root.Y;
                if (x == endX && y == endY)
                {
                    break;
                }
                Trytile(x, (short)(y - 1), endX, endY, root, 0); // 尝试向上移动
                Trytile((short)(x + 1), (short)(y - 1), endX, endY, root, 1); // 尝试向右上移动
                Trytile((short)(x + 1), y, endX, endY, root, 2); // 尝试向右移动
                Trytile((short)(x + 1), (short)(y + 1), endX, endY, root, 3); // 尝试向右下移动
                Trytile(x, (short)(y + 1), endX, endY, root, 4); // 尝试向下移动
                Trytile((short)(x - 1), (short)(y + 1), endX, endY, root, 5); // 尝试向左下移动
                Trytile((short)(x - 1), y, endX, endY, root, 6); // 尝试向左移动
                Trytile((short)(x - 1), (short)(y - 1), endX, endY, root, 7); // 尝试向左上移动
                tryCount++;
                if (tryCount > 100)
                {
                    Console.WriteLine("自动寻路算法出错,停止移动。");
                    break;
                }
            }
            for (var i = MShare.AutoPathList.Count - 1; i >= 0; i--)
            {
                Dispose(MShare.AutoPathList[i]);
            }
            MShare.AutoPathList.Clear();
            if (root == null)
            {
                FreeTree();
                return;
            }
            var temp = new FindMapNode();
            temp.X = root.X;
            temp.Y = root.Y;
            MShare.AutoPathList.Add(temp);
            int dir = root.Dir;
            var p = root;
            root = root.Father;
            while (root != null)
            {
                if (dir != root.Dir)
                {
                    temp = new FindMapNode();
                    temp.X = p.X;
                    temp.Y = p.Y;
                    MShare.AutoPathList.Insert(0, temp);
                    dir = root.Dir;
                }
                p = root;
                root = root.Father;
            }
            FreeTree();
        }

        public int RandomRange(int aFrom, int aTo)
        {
            int result;
            if (aFrom > aTo)
                result = new Random(aFrom - aTo).Next() + aTo;
            else
                result = new Random(aTo - aFrom).Next() + aFrom;
            return result;
        }

        private void InitQueue()
        {
            FreeTree();
            if (MShare.g_APQueue != null)
            {
                if (MShare.g_APQueue.Next != null)
                {
                    Dispose(MShare.g_APQueue.Next);
                }
                MShare.g_APQueue.Next = null;
                if (MShare.g_APQueue.Node != null)
                {
                    Dispose(MShare.g_APQueue.Node);
                }
                MShare.g_APQueue.Node = null;
                Dispose(MShare.g_APQueue);
                MShare.g_APQueue = null;
            }
            MShare.g_APQueue = new MapLink();
            MShare.g_APQueue.Node = null;
            MShare.g_APQueue.F = -1;
            MShare.g_APQueue.Next = new MapLink();
            MShare.g_APQueue.Next.F = 0xFFFFFFF;
            MShare.g_APQueue.Next.Node = null;
            MShare.g_APQueue.Next.Next = null;
            for (var i = MShare.AutoPathList.Count - 1; i >= 0; i--)
            {
                Dispose(MShare.AutoPathList[i]);
            }
            MShare.AutoPathList.Clear();
        }

        public void InitQueue2()
        {
            FreeTree();
            if (MShare.g_APQueue != null)
            {
                if (MShare.g_APQueue.Next != null)
                    Dispose(MShare.g_APQueue.Next);
                MShare.g_APQueue.Next = null;
                if (MShare.g_APQueue.Node != null)
                    Dispose(MShare.g_APQueue.Node);
                MShare.g_APQueue.Node = null;
                Dispose(MShare.g_APQueue);
                MShare.g_APQueue = null;
            }
            for (var i = MShare.AutoPathList.Count - 1; i >= 0; i--)
            {
                Dispose(MShare.AutoPathList[i]);
            }
            MShare.AutoPathList.Clear();
        }

        private bool IsBackToSafeZone(ref int ret)
        {
            bool result = false;
            ret = 0;
            bool has;
            if (MShare.g_gcAss[1])// 红没有回城
            {
                has = false;
                for (var i = 0; i < BotConst.MaxBagItemcl; i++)
                {
                    if (MShare.ItemArr[i].Item.Name != "" && MShare.ItemArr[i].Item.AC > 0 && MShare.ItemArr[i].Item.StdMode == 0)
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
            if (MShare.g_gcAss[2])// 蓝没有回城
            {
                has = false;
                for (var i = 0; i < BotConst.MaxBagItemcl; i++)
                {
                    if (MShare.ItemArr[i].Item.Name != "" && MShare.ItemArr[i].Item.MAC > 0 &&
                        MShare.ItemArr[i].Item.StdMode == 0)
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
            if (MShare.g_gcAss[4]) // 包裹满没有回城
            {
                has = false;
                for (var i = 0; i < 45; i++)
                {
                    if (MShare.ItemArr[i].Item.Name == "")
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
            if (MShare.g_gcAss[3])// 符没有回城
            {
                has = false;
                for (var i = 0; i < BotConst.MaxBagItemcl; i++)
                {
                    if (MShare.ItemArr[i].Item.StdMode == 25 && MShare.ItemArr[i].Item.Name != "" && MShare.ItemArr[i].Item.Name.IndexOf("符", StringComparison.Ordinal) > 0)
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
                has = false;
                for (var i = 0; i < BotConst.MaxBagItemcl; i++)
                {
                    if (MShare.ItemArr[i].Item.StdMode == 25 && MShare.ItemArr[i].Item.Name != "" && MShare.ItemArr[i].Item.Name.IndexOf("药", StringComparison.Ordinal) > 0)
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
            return result;
        }

        private int GetDis(int x1, int y1, int x2, int y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        public bool IsProperTarget(Actor actor)
        {
            return actor != null && !string.IsNullOrEmpty(actor.UserName) && !actor.Death && (actor.m_nState & PoisonState.STONEMODE) == 0 &&
                actor.UserName.IndexOf("(", StringComparison.OrdinalIgnoreCase) == -1 && actor.Visible && !actor.DelActor && !actor.m_btAFilter && MShare.g_gcAss[6] &&
                   !MShare.IgnoreMobList.ContainsKey(actor.UserName) && IsRaceProperTarget(actor);
        }

        private bool IsRaceProperTarget(Actor actor)
        {
            return actor.Race != ActorRace.Play && (actor.Race != 12 || actor.Race != 50) && actor.Race != 12 && actor.Race != ActorRace.Merchant;
        }

        /// <summary>
        /// 搜索附近的对象
        /// </summary>
        /// <returns></returns>
        private Actor SearchTarget()
        {
            Actor result = null;
            var distance = 10000;
            if (MShare.AutoTagget != null)
            {
                if (!MShare.AutoTagget.Death && MShare.AutoTagget.HiterCode == MShare.MySelf.RecogId && MShare.AutoTagget.Visible && !MShare.AutoTagget.DelActor)
                {
                    distance = GetDis(MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY, MShare.MySelf.CurrX, MShare.MySelf.CurrY);
                    result = MShare.AutoTagget;
                }
            }
            for (var i = 0; i < _robotClient.PlayScene.ActorList.Count; i++)
            {
                var actor = _robotClient.PlayScene.ActorList[i];
                if (IsProperTarget(actor))
                {
                    var dx = GetDis(actor.CurrX, actor.CurrY, MShare.MySelf.CurrX, MShare.MySelf.CurrY);
                    if (dx < distance)
                    {
                        distance = dx;
                        result = actor;
                        break;//找到一个目标跳出循环
                    }
                }
            }
            return result;
        }

        private int GetDropItemsDis()
        {
            int result = 100000;
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                TDropItem d = MShare.g_DropedItemList[i];
                if (MShare.PickUpAll || d.boPickUp)// 如果拾取过滤，则判断是否过滤
                {
                    var dx = GetDis(d.X, d.Y, MShare.MySelf.CurrX, MShare.MySelf.CurrY);
                    if (dx < result && dx != 0) // 获取距离，选择最近的
                    {
                        MShare.AutoPicupItem = d;
                        result = dx;
                    }
                }
            }
            return result;
        }

        public int GetAutoPalyStation()
        {
            int result;
            var mobdistance = 0;
            bool bPcaketfull = false;
            if (IsBackToSafeZone(ref mobdistance))
            {
                return 0;
            }
            bool has = false;
            for (var i = 0; i < 45; i++)
            {
                if (MShare.ItemArr[i] == null || MShare.ItemArr[i].Item.Name == "")
                {
                    has = true;
                    break;
                }
            }
            if (!has) // 包满
            {
                bPcaketfull = true;
            }
            if (MShare.g_nOverAPZone > 0)
            {
                return 4;
            }
            if (MShare.MapPath != null && MShare.AutoStep >= 0 && MShare.AutoStep < MShare.MapPath.Length) // 正在循路，超出范围。。。
            {
                if (MShare.AutoLastPoint.X >= 0)
                {
                    if ((Math.Abs(MShare.AutoLastPoint.X - MShare.MySelf.CurrX) >= Overdisc || Math.Abs(MShare.AutoLastPoint.X - MShare.MySelf.CurrY) >= Overdisc) &&
                        (Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrX) >= Overdisc || Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrY) >= Overdisc))
                    {
                        MShare.g_nOverAPZone = 14;
                        return 4;
                    }
                }
                else
                {
                    if (Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrX) >= Overdisc || Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrY) >= Overdisc)
                    {
                        MShare.g_nOverAPZone = 14;
                        return 4;
                    }
                }
            }
            // 获取最近的怪物
            if (MShare.AutoTagget != null)
            {
                if (MShare.AutoTagget.DelActor || MShare.AutoTagget.Death)
                {
                    MShare.AutoTagget = null;
                }
            }
            if (MShare.GetTickCount() - MShare.g_dwSearchEnemyTick > 4000 || MShare.GetTickCount() - MShare.g_dwSearchEnemyTick > 300 && MShare.AutoTagget == null)
            {
                MShare.g_dwSearchEnemyTick = MShare.GetTickCount();
                MShare.AutoTagget = SearchTarget();
            }
            if (MShare.AutoTagget != null)
            {
                if (MShare.AutoTagget.DelActor || MShare.AutoTagget.Death)
                {
                    MShare.AutoTagget = null;
                }
            }
            if (MShare.AutoTagget != null)
            {
                mobdistance = GetDis(MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY, MShare.MySelf.CurrX, MShare.MySelf.CurrY);
            }
            else
            {
                mobdistance = 100000;
            }
            // 获取最近的物品
            var itemDistance = 0;
            if (!bPcaketfull)
            {
                itemDistance = GetDropItemsDis();
            }
            else
            {
                MShare.AutoPicupItem = null;
            }
            if (itemDistance == 100000 && (mobdistance == 100000 || mobdistance == 0)) // 两者都没有发现
            {
                return 3; // 没有发现怪物或物品，随机走
            }
            if (itemDistance + 2 >= mobdistance) // 优先杀怪
            {
                result = 1; // 发现怪物
            }
            else
            {
                result = 2; // 发现物品
            }
            return result;
        }

        private bool AutoUseMagic(byte magicKey, Actor target, int nx = 0, int ny = 0)
        {
            var pcm = _robotClient.GetMagicById(magicKey);
            if (pcm == null) return false;
            MShare.FocusCret = target;
            if (nx >= 0)
            {
                _robotClient.UseMagic(nx, ny, pcm, true);
            }
            else
            {
                _robotClient.UseMagic(target.CurrX, target.CurrY, pcm);
            }
            return true;
        }

        private int TargetCount(Actor target)
        {
            var result = 1;
            var rx = target.CurrX + 1;
            var ry = target.CurrY;
            var actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX + 1;
            ry = (short)(target.CurrY + 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX + 1;
            ry = (short)(target.CurrY - 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX - 1;
            ry = target.CurrY;
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX - 1;
            ry = (short)(target.CurrY + 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX - 1;
            ry = (short)(target.CurrY - 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX;
            ry = (short)(target.CurrY + 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            rx = target.CurrX;
            ry = (short)(target.CurrY - 1);
            actor = _robotClient.PlayScene.FindActorXY(rx, ry);
            if (IsProperTarget(actor)) result++;
            return result;
        }

        private int TargetCount2(Actor target)
        {
            var result = 0;
            var wvar1 = _robotClient.PlayScene;
            for (var i = 0; i < wvar1.ActorList.Count; i++)
            {
                var actor = wvar1.ActorList[i];
                if (Math.Abs(actor.CurrX - MShare.MySelf.CurrX) < 6 ||
                    Math.Abs(actor.CurrY - MShare.MySelf.CurrY) < 6)
                    if (IsProperTarget(actor))
                        result++;
            }
            return result;
        }

        private int TargetCount3(Actor target)
        {
            var result = 0;
            var wvar1 = _robotClient.PlayScene;
            for (var i = 0; i < wvar1.ActorList.Count; i++)
            {
                var actor = wvar1.ActorList[i];
                if (Math.Abs(actor.CurrX - MShare.MySelf.CurrX) < 5 ||
                    Math.Abs(actor.CurrY - MShare.MySelf.CurrY) < 5)
                    if (IsProperTarget(actor))
                        result++;
            }

            return result;
        }

        public int TargetHumCount(Actor target)
        {
            var result = 0;
            var wvar1 = _robotClient.PlayScene;
            for (var i = 0; i < wvar1.ActorList.Count; i++)
            {
                var actor = wvar1.ActorList[i];
                if (Math.Abs(actor.CurrX - MShare.MySelf.CurrX) < 8 ||
                    Math.Abs(actor.CurrY - MShare.MySelf.CurrY) < 8)
                {
                    var b = actor != null && !actor.Death && (actor.Race == 0 || actor.m_btIsHero == 1);
                    if (b) result++;
                }
            }

            return result;
        }

        private bool AttackTaggetAAA(bool result, byte magicKey)
        {
            if (AutoUseMagic(magicKey, MShare.AutoTagget))
            {
                return result;
            }
            return result;
        }

        private bool AttackTaggetDDD()
        {
            var result = false;
            if (CanNextSpell())
            {
                byte magicKey = 0;
                if (new Random(7).Next() > 1)
                {
                    if (MShare.MagicArr[58] != null && new Random(8).Next() > 1)
                        magicKey = 58;
                    else if (MShare.MagicArr[33] != null) magicKey = 33;
                }
                else if (MShare.MagicArr[47] != null)
                {
                    magicKey = 47;
                }
                if (magicKey <= 11 && MShare.MagicArr[23] != null) magicKey = 23;
                if (magicKey > 0)
                {
                    result = true;
                    return AttackTaggetAAA(result, magicKey);
                }
                if (MShare.MagicArr[11] != null)
                    magicKey = 11;
                else if (MShare.MagicArr[5] != null)
                    magicKey = 5;
                else if (MShare.MagicArr[1] != null) magicKey = 1;
                if (magicKey > 0)
                {
                    return AttackTaggetAAA(result, magicKey);
                }
            }
            return result;
        }

        private bool AttackTaggetCCC(ref bool result)
        {
            if (MShare.GetTickCount() - MShare.m_dwSpellTick > 3000)
            {
                MShare.m_dwSpellTick = MShare.GetTickCount();
                if (MShare.MagicArr[2] != null)
                {
                    result = true;
                    if (HUtil32.Round(MShare.MySelf.Abil.HP / MShare.MySelf.Abil.MaxHP * 100) < 85)
                    {
                        if (AutoUseMagic(2, MShare.MySelf))
                        {
                            return result;
                        }
                    }
                    for (var i = 0; i < MShare.MySelf.SlaveObject.Count; i++)
                    {
                        if (MShare.MySelf.SlaveObject[i].Death) continue;
                        if (Math.Abs(MShare.MySelf.CurrX - MShare.MySelf.SlaveObject[i].CurrX + 2) <= BotConst.MagicRange && Math.Abs(MShare.MySelf.CurrY - MShare.MySelf.SlaveObject[i].CurrY + 2) <= BotConst.MagicRange)
                        {
                            if (MShare.MySelf.SlaveObject[i].Abil.HP != 0 && HUtil32.Round(MShare.MySelf.SlaveObject[i].Abil.HP / MShare.MySelf.SlaveObject[i].Abil.MaxHP * 100) < 85)
                            {
                                if (AutoUseMagic(2, MShare.MySelf.SlaveObject[i]))
                                {
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool AttackTaggetBBB(ref bool result, byte magicKey)
        {
            if (magicKey > 0)
            {
                if (AutoUseMagic(magicKey, MShare.AutoTagget)) return result;
            }
            else
            {
                result = false;
                AttackTaggetCCC(ref result);
            }
            return result;
        }

        private bool AttackTaggetEEE(ref bool result, byte magicKey)
        {
            if (CanNextSpell())
            {
                // DoubluSC
                if (MShare.MagicArr[50] != null)
                {
                    if (MShare.GetTickCount() - MShare.m_dwDoubluSCTick > 90 * 1000)
                    {
                        MShare.m_dwDoubluSCTick = MShare.GetTickCount();
                        MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                        magicKey = 50;
                        AttackTaggetBBB(ref result, magicKey);
                    }
                }
                // DECHEALTH & DAMAGEARMOR
                if (MShare.GetTickCount() - MShare.m_dwPoisonTick > 3500)
                {
                    MShare.m_dwPoisonTick = MShare.GetTickCount();
                    if (MShare.MagicArr[6] != null)
                    {
                        if ((MShare.AutoTagget.m_nState & (0x80000000)) == 0 || (MShare.AutoTagget.m_nState & (0x40000000)) == 0)
                        {
                            MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                            magicKey = 6;
                            AttackTaggetBBB(ref result, magicKey);
                        }
                    }
                    if (MShare.MagicArr[18] != null && (MShare.MySelf.m_nState & (0x00800000)) == 0 && new Random(4).Next() == 0)
                    {
                        if (TargetCount2(MShare.MySelf) >= 7)
                        {
                            MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                            magicKey = 18;
                            AttackTaggetBBB(ref result, magicKey);
                        }
                    }
                }
                if (MShare.MagicArr[13] != null || MShare.MagicArr[57] != null)
                {
                    if (MShare.MagicArr[57] != null && (HUtil32.Round(MShare.MySelf.Abil.HP / MShare.MySelf.Abil.MaxHP * 100) < 80 && new Random(100 - HUtil32.Round(MShare.MySelf.Abil.HP / MShare.MySelf.Abil.MaxHP * 100)).Next() > 5 || new Random(10).Next() > 6))
                    {
                        MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                        magicKey = 57;
                        AttackTaggetBBB(ref result, magicKey);
                    }
                    if (MShare.MagicArr[13] != null)
                    {
                        MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                        magicKey = 13;
                    }
                }
            }
            return result;
        }

        public bool AttackTagget(Actor target)
        {
            int n;
            int m;
            int tdir;
            byte magicKey;
            int i;
            int nTag;
            short nx = 0;
            short ny = 0;
            short nNx = 0;
            short nNy = 0;
            short nTx = 0;
            short nTy = 0;
            int nOldDc;
            var result = false;
            MShare.AutoMove = false;
            MShare.g_nTagCount = 0;
            if (MShare.MySelf == null || MShare.MySelf.Death || MShare.AutoTagget == null || MShare.AutoTagget.Death)
            {
                return result;
            }
            short nAbsX;
            short nAbsY;
            switch (MShare.MySelf.Job)
            {
                case 0: //战士
                    if (MShare.g_gcTec[4] && (MShare.MySelf.m_nState & 0x00100000) == 0 && CanNextSpell())
                    {
                        if (MShare.MagicArr[31] != null)
                        {
                            _robotClient.UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, MShare.MagicArr[31]);
                            return result;
                        }
                    }
                    if (_robotClient.AttackTarget(MShare.AutoTagget))
                    {
                        return true;
                    }
                    break;
                case 1: //法师
                    if (MShare.MySelf.Abil.Level < 7)
                    {
                        if (_robotClient.AttackTarget(MShare.AutoTagget))
                        {
                            result = true;
                        }
                        return result;
                    }
                    if (MShare.g_gcTec[4] && (MShare.MySelf.m_nState & 0x00100000) == 0 && CanNextSpell())
                    {
                        if (MShare.MagicArr[31] != null)
                        {
                            _robotClient.UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, MShare.MagicArr[31]);
                            return result;
                        }
                    }
                    magicKey = 11;
                    nAbsX = (short)Math.Abs(MShare.MySelf.CurrX - MShare.AutoTagget.CurrX);
                    nAbsY = (short)Math.Abs(MShare.MySelf.CurrY - MShare.AutoTagget.CurrY);
                    if (nAbsX > 2 || nAbsY > 2)
                    {
                        if (nAbsX <= BotConst.MagicRange && nAbsY <= BotConst.MagicRange)
                        {
                            result = true;
                            BotShare.logger.Info($"怪物目标：{MShare.AutoTagget.UserName} ({MShare.AutoTagget.CurrX},{MShare.AutoTagget.CurrY}) 正在使用魔法攻击");
                            if (_robotClient.CanNextAction() && _robotClient.ServerAcceptNextAction())
                                if (CanNextSpell())
                                {
                                    if (MShare.MagicArr[22] != null)
                                    {
                                        if (TargetCount3(MShare.AutoTagget) >= 15)
                                        {
                                            tdir = BotHelper.GetNextDirection(MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY, MShare.MySelf.CurrX, MShare.MySelf.CurrY);
                                            BotHelper.GetFrontPosition(MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY, tdir, ref nx, ref ny);
                                            BotHelper.GetFrontPosition(nx, ny, tdir, ref nx, ref ny);
                                            //if (robotClient.EventMan.GetEvent(nx, ny, Grobal2.ET_FIRE) == null)
                                            //{
                                            //    MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                            //    if (AutoUseMagic(22, MShare.g_APTagget, nx, ny))
                                            //    {
                                            //        return result;
                                            //    }
                                            //    result = false;
                                            //    return result;
                                            //}
                                        }
                                    }
                                    nOldDc = 3;
                                    FFFF:
                                    if (MShare.MagicArr[10] != null)
                                    {
                                        tdir = BotHelper.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);
                                        if (_robotClient.GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, 1, ref nNx, ref nNy))
                                        {
                                            _robotClient.GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, 8, ref nTx, ref nTy);
                                            if (_robotClient.CheckMagPassThrough(nNx, nNy, nTx, nTy, tdir) >= nOldDc)
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                magicKey = 10;
                                                AttackTaggetAAA(result, magicKey);
                                            }
                                        }
                                    }
                                    if (MShare.MagicArr[9] != null)
                                    {
                                        tdir = BotHelper.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);
                                        if (_robotClient.GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, 1, ref nNx, ref nNy))
                                        {
                                            _robotClient.GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, 5, ref nTx, ref nTy);
                                            if (_robotClient.CheckMagPassThrough(nNx, nNy, nTx, nTy, tdir) >= nOldDc)
                                            {
                                                MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                                magicKey = 9;
                                                AttackTaggetAAA(result, magicKey);
                                            }
                                        }
                                    }
                                    if (MShare.m_btMagPassTh > 0)
                                    {
                                        MShare.m_btMagPassTh -= 1;
                                        nOldDc = 1;
                                        goto FFFF;
                                    }

                                    if (MShare.MagicArr[11] != null)
                                        magicKey = 11;
                                    else if (MShare.MagicArr[5] != null)
                                        magicKey = 5;
                                    else if (MShare.MagicArr[1] != null) magicKey = 1;
                                    MShare.g_nTagCount = TargetCount(MShare.AutoTagget);
                                    if (MShare.g_nTagCount >= 2)
                                    {
                                        if (new Random(7).Next() > 1)
                                        {
                                            if (MShare.MagicArr[58] != null && new Random(8).Next() > 1)
                                                magicKey = 58;
                                            else if (MShare.MagicArr[33] != null) magicKey = 33;
                                        }
                                        else if (MShare.MagicArr[47] != null)
                                        {
                                            magicKey = 47;
                                        }

                                        if (magicKey <= 11 && MShare.MagicArr[23] != null) magicKey = 23;
                                    }
                                    result = false;
                                    return result;
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
                        if (nAbsX <= 1 && nAbsY <= 1)// 目标近身
                        {
                            if (CanNextSpell())
                            {
                                nTag = TargetCount(MShare.MySelf);
                                if (nTag >= 5)// 怪太多,强攻解围...
                                {
                                    AttackTaggetDDD();
                                    return false;// 躲避
                                }
                                if (nTag >= 4 && MShare.MagicArr[8] != null) // 比较勉强的抗拒...一般选择逃避
                                {
                                    result = true;
                                    MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                    if (AutoUseMagic(8, MShare.MySelf))
                                    {
                                        if (MShare.m_btMagPassTh <= 0) MShare.m_btMagPassTh += 1 + RandomNumber.GetInstance().Random(2);
                                        return result;
                                    }
                                }
                            }
                        }
                        tdir = BotHelper.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);// 避怪
                        BotHelper.GetBackPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nx, ref ny);
                        nTag = 0;
                        while (true)
                        {
                            if (_robotClient.PlayScene.CanWalk(nx, ny)) break;
                            tdir++;
                            tdir = tdir % 8;
                            BotHelper.GetBackPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nx, ref ny);
                            nTag++;
                            if (nTag > 8) break;
                        }
                        if (_robotClient.PlayScene.CanWalk(nx, ny))
                        {
                            BotHelper.GetBackPosition2(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nTx, ref nTy);
                            // Map.CanMove(nTX, nTY)
                            if (_robotClient.PlayScene.CanWalk(nTx, nTy))
                            {
                                // DScreen.AddChatBoardString(Format('避怪2(%d:%d)...........', [nTX, nTY]), clBlue, clWhite);
                                MShare.TargetX = nTx;
                                MShare.TargetY = nTy;
                                MShare.PlayerAction = PlayerAction.Run;
                                MShare.MouseCurrX = nTx;
                                MShare.MouseCurrY = nTy;
                                result = true;
                            }
                            else
                            {
                                // DScreen.AddChatBoardString(Format('避怪(%d:%d)...........', [nX, nY]), clBlue, clWhite);
                                MShare.TargetX = nx;
                                MShare.TargetY = ny;
                                MShare.PlayerAction = PlayerAction.Run;
                                MShare.MouseCurrX = nx;
                                MShare.MouseCurrY = ny;
                                result = true;
                            }
                        }
                        else
                        {
                            // 强攻
                            // DScreen.AddChatBoardString('强攻...........', clBlue, clWhite);
                            AttackTaggetDDD();
                        }
                    }
                    break;
                case 2://道士
                    if (MShare.g_gcTec[4] && (MShare.MySelf.m_nState & 0x00100000) == 0 && CanNextSpell())
                    {
                        if (MShare.MagicArr[31] != null)
                        {
                            _robotClient.UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, MShare.MagicArr[31]);
                            return result;
                        }
                    }
                    n = 0;
                    if (MShare.UseItems[ItemLocation.ArmRingl].Item.StdMode == 25 && MShare.UseItems[ItemLocation.ArmRingl].Item.Shape != 6 && MShare.UseItems[ItemLocation.ArmRingl].Item.Name.IndexOf("药", StringComparison.Ordinal) > 0)
                    {
                        n++;
                    }
                    if (n == 0)
                    {
                        for (i = 6; i < BotConst.MaxBagItemcl; i++)
                        {
                            if (MShare.ItemArr[i].Item.NeedIdentify < 4 && MShare.ItemArr[i].Item.StdMode == 25 && MShare.ItemArr[i].Item.Shape != 6 && MShare.ItemArr[i].Item.Name.IndexOf("药", StringComparison.Ordinal) > 0)
                            {
                                n++;
                                break;
                            }
                        }
                    }
                    if (n == 0)
                    {
                        if (MShare.GetTickCount() - _gHinttick1 > 60 * 1000)
                        {
                            _gHinttick1 = MShare.GetTickCount();
                            ScreenManager.AddChatBoardString("你的[药粉]已经用完，注意补充");
                        }
                    }
                    m = 0;
                    if (MShare.UseItems[ItemLocation.ArmRingl].Item.StdMode == 25 && MShare.UseItems[ItemLocation.ArmRingl].Item.Shape != 6 && MShare.UseItems[ItemLocation.ArmRingl].Item.Name.IndexOf("符", StringComparison.Ordinal) > 0)
                    {
                        m++;
                    }
                    if (m == 0)
                    {
                        for (i = 6; i < BotConst.MaxBagItemcl; i++)
                        {
                            if (MShare.ItemArr[i].Item.NeedIdentify < 4 && MShare.ItemArr[i].Item.StdMode == 25 && MShare.ItemArr[i].Item.Shape != 6 && MShare.ItemArr[i].Item.Name.IndexOf("符", StringComparison.Ordinal) > 0)
                            {
                                m++;
                                break;
                            }
                        }
                    }
                    if (m == 0)
                    {
                        if (MShare.GetTickCount() - _gHinttick2 > 60 * 1000)
                        {
                            _gHinttick2 = MShare.GetTickCount();
                            ScreenManager.AddChatBoardString("你的[护身符]已经用完，注意补充");
                        }
                    }
                    if (MShare.GetTickCount() - MShare.m_dwRecallTick > 1000 * 6)
                    {
                        // 设置比较大时间,以便其他攻击...
                        MShare.m_dwRecallTick = MShare.GetTickCount();
                        if (MShare.MySelf.SlaveObject.Count == 0 && m > 0)
                        {
                            magicKey = 0;
                            if (MShare.MagicArr[55] != null)
                                magicKey = 55;
                            else if (MShare.MagicArr[30] != null)
                                magicKey = 30;
                            else if (MShare.MagicArr[17] != null)
                                magicKey = 17;

                            if (magicKey != 0)
                            {
                                result = true;
                                var pcm = _robotClient.GetMagicById(magicKey);
                                if (pcm == null)
                                {
                                    result = false;
                                    return result;
                                }
                                MShare.FocusCret = null;
                                tdir = BotHelper.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);
                                BotHelper.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nx, ref ny);
                                _robotClient.UseMagic(nx, ny, pcm, true);
                                return result;
                            }
                        }
                    }
                    if (MShare.GetTickCount() - MShare.m_dwSpellTick > 1000 * 5)
                    {
                        // 状态类魔法...
                        MShare.m_dwSpellTick = MShare.GetTickCount();
                        // MAGDEFENCEUP
                        if (MShare.MagicArr[14] != null && m > 0)
                        {
                            if ((MShare.MySelf.m_nState & (0x00200000)) == 0)
                            {
                                result = true;
                                if (AutoUseMagic(14, MShare.MySelf)) return result;
                            }
                        }
                        // Double DefenceUp
                        if (MShare.MagicArr[15] != null && m > 0)
                        {
                            if ((MShare.MySelf.m_nState & 0x00400000) == 0)
                            {
                                result = true;
                                if (AutoUseMagic(15, MShare.MySelf)) return result;
                            }
                        }
                        // Healling
                        if (MShare.MagicArr[2] != null)
                        {
                            if (HUtil32.Round(MShare.MySelf.Abil.HP / MShare.MySelf.Abil.MaxHP * 100) < 85)
                            {
                                result = true;
                                if (AutoUseMagic(2, MShare.MySelf)) return result;
                            }
                            if (MShare.MySelf.SlaveObject.Count > 0)
                            {
                                for (i = 0; i < MShare.MySelf.SlaveObject.Count; i++)
                                {
                                    if (MShare.MySelf.SlaveObject[i].Death) continue;
                                    if (MShare.MySelf.SlaveObject[i].Abil.HP != 0 && Math.Abs(MShare.MySelf.CurrX - MShare.MySelf.SlaveObject[i].CurrX + 2) <= BotConst.MagicRange
                                                                                        && Math.Abs(MShare.MySelf.CurrY - MShare.MySelf.SlaveObject[i].CurrY + 2) <= BotConst.MagicRange)
                                    {
                                        if (HUtil32.Round(MShare.MySelf.SlaveObject[i].Abil.HP / MShare.MySelf.SlaveObject[i].Abil.MaxHP * 100) <= 80)
                                        {
                                            result = true;
                                            if (AutoUseMagic(2, MShare.MySelf.SlaveObject[i])) return result;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (MShare.MySelf.Abil.Level < 18 || MShare.MagicArr[13] == null || n == 0 && m == 0)
                    {
                        AttackTaggetCCC(ref result);
                        if (_robotClient.AttackTarget(MShare.AutoTagget)) result = true;
                        return result;
                    }
                    magicKey = 0;
                    nAbsX = (short)Math.Abs(MShare.MySelf.CurrX - MShare.AutoTagget.CurrX);
                    nAbsY = (short)Math.Abs(MShare.MySelf.CurrY - MShare.AutoTagget.CurrY);
                    if (nAbsX > 2 || nAbsY > 2)
                    {
                        // 需要快速检测类...
                        if (nAbsX <= BotConst.MagicRange && nAbsY <= BotConst.MagicRange)
                        {
                            result = true;
                            BotShare.logger.Info($"怪物目标：{MShare.AutoTagget.UserName} ({MShare.AutoTagget.CurrX},{MShare.AutoTagget.CurrY}) 正在使用魔法攻击");
                            if (_robotClient.CanNextAction() && _robotClient.ServerAcceptNextAction())
                            {
                                AttackTaggetEEE(ref result, magicKey);
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
                        if (nAbsX <= 1 && nAbsY <= 1)// 目标近身
                            if (CanNextSpell())
                            {
                                nTag = TargetCount(MShare.MySelf);
                                if (nTag >= 5)// 怪太多,强攻解围...
                                {
                                    AttackTaggetEEE(ref result, magicKey);
                                }
                                if (MShare.MagicArr[48] != null && nTag >= 3)
                                {
                                    // 有力的抗拒...不同于法师逃避
                                    result = true;
                                    MShare.m_dwTargetFocusTick = MShare.GetTickCount();
                                    if (AutoUseMagic(48, MShare.MySelf))
                                    {
                                        if (MShare.m_btMagPassTh <= 0) MShare.m_btMagPassTh += 1 + RandomNumber.GetInstance().Random(2);
                                        return result;
                                    }
                                }
                            }

                        tdir = BotHelper.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);
                        BotHelper.GetBackPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nx, ref ny); // 避怪
                        nTag = 0;
                        while (true)
                        {
                            if (_robotClient.PlayScene.CanWalk(nx, ny)) break;
                            tdir++;
                            tdir = tdir % 8;
                            BotHelper.GetBackPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nx, ref ny);
                            nTag++;
                            if (nTag > 8) break;
                        }

                        if (_robotClient.PlayScene.CanWalk(nx, ny))
                        {
                            BotHelper.GetBackPosition2(MShare.MySelf.CurrX, MShare.MySelf.CurrY, tdir, ref nTx, ref nTy);
                            // Map.CanMove(nTX, nTY)
                            if (_robotClient.PlayScene.CanWalk(nTx, nTy))
                            {
                                // DScreen.AddChatBoardString(Format('避怪2(%d:%d)...........', [nTX, nTY]), clBlue, clWhite);
                                MShare.TargetX = nTx;
                                MShare.TargetY = nTy;
                                MShare.PlayerAction = PlayerAction.Run;
                                MShare.MouseCurrX = nTx;
                                MShare.MouseCurrY = nTy;
                                result = true;
                            }
                            else
                            {
                                // DScreen.AddChatBoardString(Format('避怪(%d:%d)...........', [nX, nY]), clBlue, clWhite);
                                MShare.TargetX = nx;
                                MShare.TargetY = ny;
                                MShare.PlayerAction = PlayerAction.Run;
                                MShare.MouseCurrX = nx;
                                MShare.MouseCurrY = ny;
                                result = true;
                            }
                        }
                        else
                        {
                            // 强攻
                            // DScreen.AddChatBoardString('强攻...........', clBlue, clWhite);
                            AttackTaggetEEE(ref result, magicKey);
                        }
                    }
                    break;
            }
            return result;
        }

        private void Dispose(object obj)
        {
        }

    }
}