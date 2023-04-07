using GameSrv.Maps;

namespace GameSrv.Event.Events {
    /// <summary>
    /// 矿石事件
    /// </summary>
    public class StoneMineEvent : MapEvent {
        private readonly int AddStoneCount;
        public int MineCount;
        public int AddStoneMineTick;
        public bool AddToMap;

        public StoneMineEvent(Envirnoment Envir, short nX, short nY, byte nType) : base(Envir, nX, nY, nType, 0, false) {
            AddToMap = true;
            if (nType is 55 or 56 or 57) {
                if (Envir.AddToMapItemEvent(nX, nY, CellType.Event, this)) {
                    Visible = false;
                    MineCount = M2Share.RandomNumber.Random(2000) + 300;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    AddStoneCount = M2Share.RandomNumber.Random(800) + 100;
                }
                else {
                    AddToMap = false;
                }
            }
            else {
                if (Envir.AddToMapMineEvent(nX, nY, CellType.Event, this)) {
                    Visible = false;
                    MineCount = M2Share.RandomNumber.Random(200) + 1;
                    AddStoneMineTick = HUtil32.GetTickCount();
                    Active = false;
                    AddStoneCount = M2Share.RandomNumber.Random(80) + 1;
                }
                else {
                    AddToMap = false;
                }
            }
        }

        public void AddStoneMine() {
            MineCount = AddStoneCount;
            AddStoneMineTick = HUtil32.GetTickCount();
        }
    }
}