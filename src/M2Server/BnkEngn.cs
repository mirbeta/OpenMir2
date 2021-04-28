using System.Collections;

namespace M2Server
{
    public struct TReQuestInfo
    {
        public TMerchant NPC;
        public TPlayObject PlayObject;
        public TOpAction OpAction;
        public int nGameGold;
        public string sAccount;
        public string sPassword;
    }

    public class TBankEngine
    {
        public ArrayList m_UserReQuestList = null;
        public ArrayList m_CompleteList = null;
        public object m_CS = null;

        public TBankEngine()
        {
            m_CS = new object();
            m_UserReQuestList = new ArrayList();
            m_CompleteList = new ArrayList();
        }

        public void Execute()
        {
            //while (true)
            //{
            //    __Lock();
            //    try
            //    {
            //    }
            //    finally
            //    {
            //        UnLock();
            //    }
            //    for (var I = 0; I < m_UserReQuestList.Count; I++)
            //    {
            //        ReQuestInfo = m_UserReQuestList[I];
            //        switch (ReQuestInfo.OpAction)
            //        {
            //            case TOpAction.o_GetGold:
            //                break;
            //            case TOpAction.o_SaveGold:
            //                break;
            //            case TOpAction.o_ViewGold:
            //                break;
            //        }
            //    }
            //    this.Sleep(1);
            //}
        }

        public void __Lock()
        {
          HUtil32.EnterCriticalSection(m_CS);
        }

        public void UnLock()
        {
            HUtil32.LeaveCriticalSection(m_CS);
        }
    }

    public enum TOpAction
    {
        o_GetGold,
        o_SaveGold,
        o_ViewGold
    }
}

