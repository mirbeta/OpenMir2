using OpenMir2.Packets.ServerPackets;
using SystemModule.Actors;

namespace SystemModule.Data
{
    public class SavePlayerRcd
    {
        public string Account;
        public string ChrName;
        public int SessionID;
        public IPlayerActor PlayObject;
        public CharacterDataInfo CharacterData;
        public int ReTryCount;
        public bool IsSaveing;
        public int QueryId;

        public SavePlayerRcd()
        {
            CharacterData = new CharacterDataInfo();
        }
    }
}
