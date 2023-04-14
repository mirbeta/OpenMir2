namespace DBSrv.Services
{
    public interface IService
    {
        void Initialize();
        
        void Start();

        void Stop();
    }
}