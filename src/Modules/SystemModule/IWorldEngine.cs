using SystemModule.Data;
using SystemModule.Enums;

namespace SystemModule
{
    public interface IWorldEngine
    {
        void Initialize();

        void InitializationMonsterThread();

        void InitializeMonster();

        IPlayerActor GetPlayObject(string chrName);

        IEnumerable<IPlayerActor> PlayObjects { get; }

        int MonsterCount { get; }

        int MagicCount { get; }

        int OfflinePlayCount { get; }

        int RobotPlayerCount { get; }

        int RobotLogonQueueCount { get; }

        int MonGenCount { get; }

        void CryCry(short wIdent, IEnvirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg);

        void GetMapRageHuman(IEnvirnoment envir, int nRageX, int nRageY, int nRage, ref IList<IActor> list, bool botPlay = false);

        int GetMapOfRangeHumanCount(IEnvirnoment envir, int nX, int nY, int nRange);

        IActor RegenMonsterByName(string sMap, short nX, short nY, string sMonName);

        void SendBroadCastMsg(string sMsg, MsgType msgType);

        void SendBroadCastMsgExt(string sMsg, MsgType msgType);

        IMerchant FindMerchant(int npcId);

        INormNpc FindNpc(int npcId);

        MagicInfo FindMagic(int nMagIdx);

        MagicInfo FindMagic(string sMagicName);

        IList<IMerchant> MerchantList { get; set; }

        void AddMerchant(IMerchant merchant);

        IList<AdminInfo> AdminList { get; set; }

        int PlayObjectCount { get; }

        int OnlinePlayObject { get; }

        int GetPlayExpireTime(string account);

        void SetPlayExpireTime(string account, int expiredTime);

        void AccountExpired(string account);

        void SendServerGroupMsg(int nCode, int nServerIdx, string sMsg);

        bool FindOtherServerUser(string mapName, ref int srvIdx);

        int GetMonstersZenTime(int time);

        int GetMapHuman(string mapName);

        int GetMapMonster(IEnvirnoment envir, IList<IActor> list);

        void AddMonGenList(MonGenInfo monGenInfo);

        bool CheckMonGenInfoThreadMap(int threadId);

        void AddMonGenInfoThreadMap(int threadId, MonGenInfo monGenInfo);

        void CreateMonGenInfoThreadMap(int threadId, IList<MonGenInfo> monGenInfo);

        void ClearMonsterList();

        void AddMonsterList(MonsterInfo monsterInfo);

        void SwitchMagicList();

        void AddMagicList(MagicInfo magicInfo);

        void PrcocessData();
        void ProcessNpcs();
        void ProcessMerchants();
        void ProcessRobotPlayData();
        void ProcessHumans();
    }
}
