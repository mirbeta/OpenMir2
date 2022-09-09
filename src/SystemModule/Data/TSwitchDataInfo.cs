using System.Collections.Generic;
using SystemModule.Packet.ClientPackets;

namespace SystemModule.Data
{
    public class SwitchDataInfo
    {
        public string sMap;
        public short wX;
        public short wY;
        public Ability Abil;
        public string sChrName;
        public int nCode;
        public bool boC70;
        public bool boBanShout;
        public bool boHearWhisper;
        public bool boBanGuildChat;
        public bool boAdminMode;
        public bool boObMode;
        public IList<string> BlockWhisperArr;
        public SlaveInfo[] SlaveArr;
        public ushort[] StatusValue;
        public int[] StatusTimeOut;
        public int dwWaitTime;
    }
}