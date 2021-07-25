namespace M2Server
{
    public class SendMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int dwDeliveryTime;
        public TBaseObject BaseObject;
        public bool boLateDelivery;
        public string Buff;
    }

    public class TVisibleBaseObject
    {
        public TBaseObject BaseObject;
        public int nVisibleFlag;
    }

    public class TVisibleMapItem
    {
        public int nX;
        public int nY;
        public TMapItem MapItem;
        public string sName;
        public ushort wLooks;
        public int nVisibleFlag;
    } 
}

namespace M2Server
{
    public class ObjBase
    {
        public const int gMan = 0;
        public const int gWoMan = 1;

        public static void AddUserLog(string sMsg)
        {
            M2Share.MainOutMessage(sMsg);
        }
    }
}

