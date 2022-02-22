using SystemModule;

namespace MakePlayer
{
    public class ClientManager
    {
        private IList<TObjClient> _Clients;
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        private int dwRunTick = 0;

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
            dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            for (var i = g_nPosition; i < _Clients.Count; i++)
            {
                _Clients[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 20))
                {
                    g_nPosition = i;
                    boProcessLimit = true;
                    break;
                }
                if (_Clients[i].m_boLogin && (HUtil32.GetTickCount() - _Clients[i].m_dwSayTick > 3000))
                {
                    _Clients[i].m_dwSayTick = HUtil32.GetTickCount();
                    _Clients[i].ClientLoginSay();
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