using OpenMir2.Data;
using SystemModule.Actors;
using SystemModule.Data;
using SystemModule.MagicEvent;
using SystemModule.MagicEvent.Events;

namespace SystemModule.Maps
{
    public interface IEnvirnoment
    {
        byte ServerIndex { get; set; }

        int EnterLevel { get; }

        short MinMap { get; set; }

        string MapName { get; }

        string MapFileName { get; set; }

        string MapDesc { get; set; }

        short Width { get; }

        short Height { get; }

        MapInfoFlag Flag { get; }


        int HumCount { get; }

        int MonCount { get; }

        bool ChFlag { get; set; }

        IList<PointInfo> PointList { get; set; }

        IList<MapDoor> DoorList { get; }

        void AddDoorToMap();

        bool AddItemToMap(int nX, int nY, MapItem mapItem);

        void AddMapDoor(int nX, int nY, MapDoor mapDoor);

        void AddMapEvent(int nX, int nY, MapEvent mapEvent);

        bool AddMapObject(int nX, int nY, CellType cellType, int cellId, IActor mapObject);

        void AddMapRoute(int nX, int nY, MapRouteItem mapRoute);

        bool AddMineToEvent<T>(int nX, int nY, T stoneMineEvent) where T : StoneMineEvent;

        void AddObject(IActor baseObject);

        bool AddToMapMineEvent<T>(int nX, int nY, T stoneMineEvent) where T : StoneMineEvent;

        bool ArroundDoorOpened(int nX, int nY);

        bool CanFly(int nsX, int nsY, int ndX, int ndY);

        bool CanSafeWalk(int nX, int nY);

        bool CanWalk(int nX, int nY);

        bool CanWalk(int nX, int nY, bool boFlag);

        bool CanWalkEx(int nX, int nY, bool boFlag);

        bool CanWalkOfItem(int nX, int nY, bool boFlag, bool boItem);

        bool CellMatch(int nX, int nY);

        bool CellValid(int nX, int nY);

        int DeleteFromMap(int nX, int nY, CellType cellType, int cellId, IActor mapObject);

        void DelObjectCount(IActor baseObject);

        void GetBaseObjects(int nX, int nY, bool boFlag, ref IList<IActor> actorList);

        ref MapCellInfo GetCellInfo(int nX, int nY, out bool success);

        bool GetDoor(int nX, int nY, ref MapDoor door);

        string GetEnvirInfo();

        MapEvent GetEvent(int nX, int nY);

        bool GetItem(int nX, int nY, ref MapItem mapItem);

        int GetItemEx(int nX, int nY, ref int nCount);

        IActor GetMovingObject(short nX, short nY, bool boFlag);

        bool GetNextPosition(short sx, short sy, byte ndir, int nFlag, ref short snx, ref short sny);

        int GetRangeBaseObject(int nX, int nY, int nRage, bool boFlag, IList<IActor> actorList);

        bool GetXyHuman(int nMapX, int nMapY);

        int GetXYObjCount(int nX, int nY);

        bool IsCheapStuff();

        bool IsValidCell(int nX, int nY);

        bool IsValidObject(int nX, int nY, int nRage, IActor baseObject);

        bool LoadMapData(string sMapFile);

        bool MoveToMovingObject(int nCx, int nCy, IActor cert, int nX, int nY, bool boFlag);

        void SetMapXyFlag(int nX, int nY, bool boFlag);

        void VerifyMapTime(int nX, int nY, IActor baseObject);

        void Dispose();
    }
}