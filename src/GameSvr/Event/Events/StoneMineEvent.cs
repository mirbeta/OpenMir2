using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Event.Events
{
    public class StoneMineEvent : MirEvent
    {
        private readonly int _addStoneCount = 0;
        public int MineCount = 0;
        public int AddStoneMineTick = 0;
        public bool AddToMap = false;

        public StoneMineEvent(Envirnoment Envir, int nX, int nY, int nType) : base(Envir, nX, nY, nType, 0, false)
        {
            AddToMap = true;
            if (nType is 55 or 56 or 57)
            {
                if (m_Envir.AddToMapItemEvent(nX, nY, CellType.EventObject, this) == null)
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
                if (m_Envir.AddToMapMineEvent(nX, nY, CellType.EventObject, this) == null)
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