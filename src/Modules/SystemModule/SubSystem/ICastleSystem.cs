using SystemModule.Actors;
using SystemModule.Castles;
using SystemModule.Maps;

namespace SystemModule.SubSystem
{
    public interface ICastleSystem
    {
        IUserCastle Find(string sCastleName);
        IUserCastle GetCastle(int nIndex);
        void GetCastleGoldInfo(IList<string> List);
        void GetCastleNameList(IList<string> List);
        IUserCastle InCastleWarArea(IEnvirnoment Envir, int nX, int nY);
        IUserCastle InCastleWarArea(IActor BaseObject);
        void IncRateGold(int nGold);
        void Initialize();
        IUserCastle IsCastleEnvir(IEnvirnoment envir);
        IUserCastle IsCastleMember(IPlayerActor playObject);
        IUserCastle IsCastlePalaceEnvir(IEnvirnoment Envir);
        void LoadCastleList();
        void Run();
        void Save();
    }
}