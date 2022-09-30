using MySqlConnector;
using NLog;
using System;
using System.Data;

namespace DBSvr.Storage.MySQL
{
    public class DBContext : IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly StorageOption _storageOption;
        private MySqlConnection? _connection;
        private MySqlTransaction? _transaction;

        public DBContext(StorageOption storageOption)
        {
            _storageOption = storageOption;
        }

        public void Open(ref bool succes)
        {
            _connection = new MySqlConnection(_storageOption.ConnectionString);
            try
            {
                _connection.Open();
                succes = true;
            }
            catch (Exception e)
            {
                _logger.Error("打开数据库[MySql]失败.");
                _logger.Error(e.StackTrace);
                succes = false;
            }
        }

        public MySqlCommand CreateCommand()
        {
            return new MySqlCommand(connection: _connection, transaction: _transaction);
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public void BeginTransaction()
        {
            if (_transaction == null && _connection.State == ConnectionState.Open)
            {
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            if (_transaction == null)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}