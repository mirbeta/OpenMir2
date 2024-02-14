using OpenMir2;
using SystemModule.Maps;

namespace SystemModule.MagicEvent.Events
{
    /// <summary>
    /// 矿石事件
    /// </summary>
    public class StoneMineEvent : MapEvent
    {
        private readonly int _addStoneCount;
        public readonly bool AddToMap;
        public int MineCount;
        public int AddStoneMineTick;

        public StoneMineEvent(IEnvirnoment envir, short nX, short nY, byte nType) : base(envir, nX, nY, nType, 0, false)
        {
            AddToMap = true;
            if (nType is 55 or 56 or 57)
            {
                if (envir.AddMineToEvent(nX, nY, this))
                {
                    Visible = false;
                    MineCount = SystemShare.RandomNumber.Random(2000) + 300;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    _addStoneCount = SystemShare.RandomNumber.Random(800) + 100;
                }
                else
                {
                    AddToMap = false;
                }
            }
            else
            {
                if (envir.AddToMapMineEvent(nX, nY, this))
                {
                    Visible = false;
                    MineCount = SystemShare.RandomNumber.Random(200) + 1;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    _addStoneCount = SystemShare.RandomNumber.Random(80) + 1;
                }
                else
                {
                    AddToMap = false;
                }
            }
        }

        public void AddStoneMine()
        {
            MineCount = _addStoneCount;
            AddStoneMineTick = HUtil32.GetTickCount();
        }
    }
}