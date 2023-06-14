using System.Collections;
using SystemModule.Data;

namespace M2Server
{
    public interface IAccountSessionService
    {
        void CheckConnected();
        void Close();
        PlayerSession GetAdmission(string sAccount, string sIPaddr, int nSessionID, ref int nPayMode, ref int nPayMent, ref long playTime);
        int GetSessionCount();
        void GetSessionList(ArrayList List);
        void Initialize();
        void Run();
        void SendHumanLogOutMsg(string sUserId, int nId);
        void SendHumanLogOutMsgA(string sUserID, int nID);
        void SendLogonCostMsg(string sAccount, int nTime);
        void SendOnlineHumCountMsg(int nCount);
        void SendSocket(string sSendMsg);
        void SendUserPlayTime(string account, long playTime);
    }
}