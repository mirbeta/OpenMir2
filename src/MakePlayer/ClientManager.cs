using SystemModule;

namespace MakePlayer
{
    public class ClientManager
    {
        private IList<TObjClient> _Clients;
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        
        public ClientManager()
        {
            _Clients = new List<TObjClient>();
        }

        public void AddClient(TObjClient objClient)
        {
            _Clients.Add(objClient);
        }

        public void DelClient(TObjClient objClient)
        {
            _Clients.Remove(objClient);
        }

        public void Run()
        {
            var dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            var clientList = _Clients;
            for (var i = g_nPosition; i < clientList.Count; i++)
            {
                clientList[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 20))
                {
                    g_nPosition = i;
                    boProcessLimit = true;
                    break;
                }
                if (clientList[i].m_boLogin && (HUtil32.GetTickCount() - clientList[i].m_dwSayTick > 3000))
                {
                    clientList[i].m_dwSayTick = HUtil32.GetTickCount();
                    clientList[i].ClientLoginSay();
                }
            }
            if (!boProcessLimit)
            {
                g_nPosition = 0;
            }
            g_dwProcessTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (g_dwProcessTimeMin > g_dwProcessTimeMax)
            {
                g_dwProcessTimeMax = g_dwProcessTimeMin;
            }
        }
    }
}