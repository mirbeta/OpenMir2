using M2Server.Actor;
using M2Server.Npc;
using M2Server.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.Word
{
    public interface IWordEngine
    {
        PlayObject GetPlayObject(string chrName);

        void CryCry(short wIdent, Envirnoment pMap, int nX, int nY, int nWide, byte btFColor, byte btBColor, string sMsg);

        void GetMapRageHuman(Envirnoment envir, int nRageX, int nRageY, int nRage, ref IList<BaseObject> list, bool botPlay = false);

        int GetMapOfRangeHumanCount(Envirnoment envir, int nX, int nY, int nRange);

        BaseObject RegenMonsterByName(string sMap, short nX, short nY, string sMonName);

        void SendBroadCastMsg(string sMsg, MsgType msgType);

        void SendBroadCastMsgExt(string sMsg, MsgType msgType);

        IMerchant FindMerchant(int npcId);

        NormNpc FindNpc(int npcId);

        MagicInfo FindMagic(int nMagIdx);

        MagicInfo FindMagic(string sMagicName);

        int PlayObjectCount { get; }

        int OnlinePlayObject { get; }

        int GetPlayExpireTime(string account);

        void SetPlayExpireTime(string account, int expiredTime);

        void AccountExpired(string account);

        void SendServerGroupMsg(int nCode, int nServerIdx, string sMsg);

        bool FindOtherServerUser(string mapName, ref int srvIdx);

        int GetMonstersZenTime(long time);

        int GetMapHuman(string mapName);

        int GetMapMonster(Envirnoment envir, IList<BaseObject> list);
    }
}
