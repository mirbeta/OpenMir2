using System.Collections;
using SystemModule.Data;

namespace M2Server
{
    public interface ILoginSession
    {
        void Initialize();

        void Run();

        void CheckConnected();

        void Close();

        PlayerSession GetAdmission(string account, string paddr, int sessionId, ref int payMode, ref int payMent, ref long playTime);

        int GetSessionCount();

        void GetSessionList(ArrayList list);

        void SendHumanLogOutMsg(string userId, int nId);

        void SendHumanLogOutMsgA(string userId, int nId);

        void SendLogonCostMsg(string account, int nTime);

        void SendOnlineHumCountMsg(int nCount);

        void SendSocket(string sendMsg);

        void SendUserPlayTime(string account, long playTime);
    }
}