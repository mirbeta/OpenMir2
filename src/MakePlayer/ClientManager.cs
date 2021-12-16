namespace MakePlayer
{
    public class ClientManager
    {
        public IList<TObjClient> _Clients;

        public ClientManager()
        {
            _Clients = new List<TObjClient>();
        }

        public IList<TObjClient> List { get { return _Clients; } }

        public int Count { get { return _Clients.Count; } }

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
            for (int i = 0; i < _Clients.Count; i++)
            {
                _Clients[i].Run();
            }
        }
    }
}