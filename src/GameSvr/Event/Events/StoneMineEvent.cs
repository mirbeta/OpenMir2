using GameSvr.Maps;

namespace GameSvr.Event.Events
{
    public class StoneMineEvent : EventInfo
    {
        private readonly int _addStoneCount;
        public int MineCount;
        public int AddStoneMineTick;
        public bool AddToMap;

        public StoneMineEvent(Envirnoment Envir, short nX, short nY, byte nType) : base(Envir, nX, nY, nType, 0, false)
        {
            AddToMap = true;
            if (nType is 55 or 56 or 57)
            {
                if (!Envir.AddToMapItemEvent(nX, nY, CellType.Event, this))
                {
                    AddToMap = false;
                }
                else
                {
                    Visible = false;
                    MineCount = M2Share.RandomNumber.Random(2000) + 300;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    _addStoneCount = M2Share.RandomNumber.Random(800) + 100;
                }
            }
            else
            {
                if (this.Envir.AddToMapMineEvent(nX, nY, CellType.Event, this) == null)
                {
                    AddToMap = false;
                }
                else
                {
                    Visible = false;
                    MineCount = M2Share.RandomNumber.Random(200) + 1;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    _addStoneCount = M2Share.RandomNumber.Random(80) + 1;
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