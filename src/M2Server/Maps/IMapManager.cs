using SystemModule;
using SystemModule.Data;

namespace M2Server
{
    public interface IMapManager
    {
        IList<IEnvirnoment> Maps { get; }
        void AddMapInfo(string sMapName, string sMapDesc, byte nServerNumber, MapInfoFlag mapFlag, IMerchant questNpc);
        bool AddMapRoute(string sSMapNo, int nSMapX, int nSMapY, string sDMapNo, int nDMapX, int nDMapY);
        IEnvirnoment FindMap(string sMapName);
        IList<IEnvirnoment> GetDoorMapList();
        IEnvirnoment GetMapInfo(int nServerIdx, string sMapName);
        int GetMapOfServerIndex(string sMapName);
        IList<IEnvirnoment> GetMineMaps();
        void LoadMapDoor();
        void MakeSafePkZone();
    }
}