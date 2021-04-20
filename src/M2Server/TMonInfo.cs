using System.Collections.Generic;

namespace M2Server
{
    public struct TMonInfo
    {
        public IList<TMonItem> ItemList;
        public string sName;
        public byte btRace;
        public byte btRaceImg;
        public short wAppr;
        public short wLevel;
        public byte btLifeAttrib;
        public short wCoolEye;
        public int dwExp;
        public short wHP;
        public short wMP;
        public short wAC;
        public short wMAC;
        public short wDC;
        public short wMaxDC;
        public short wMC;
        public short wSC;
        public short wSpeed;
        public short wHitPoint;
        public short wWalkSpeed;
        public short wWalkStep;
        public short wWalkWait;
        public short wAttackSpeed;
        public short wAntiPush;
        public bool boAggro;
        public bool boTame;
    }
}