namespace DBSrv.Storage
{
    public class StorageOption
    {
        public StorageOption(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}