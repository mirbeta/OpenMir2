using System;

namespace GameSvr
{
    public class Castle
    {
        public const int MAXCASTLEARCHER = 12;
        public const int MAXCALSTEGUARD = 4;
    }

    public class TAttackerInfo
    {
        public DateTime AttackDate;
        public string sGuildName;
        public Association Guild;
    }

    public struct TDefenseUnit
    {
        public int nMainDoorX;
        public int nMainDoorY;
        public string sMainDoorName;
        public bool boXXX;
        public ushort wMainDoorHP;
        public TBaseObject MainDoor;
        public TBaseObject LeftWall;
        public TBaseObject CenterWall;
        public TBaseObject RightWall;
        public TBaseObject Archer;
    }

    public class TObjUnit
    {
        public short nX;
        public short nY;
        public string sName;
        public bool nStatus;
        public ushort nHP;
        public TBaseObject BaseObject;
    }
}

