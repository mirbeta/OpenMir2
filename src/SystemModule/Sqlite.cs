using System.Data;
using System.Data.SQLite;

namespace SystemModule
{
    public class Sqlite
    {
        public static string SqlConnctionString = "Data Source={0}";

        public Sqlite(string DataPath)
        {
            SqlConnctionString = string.Format(SqlConnctionString, DataPath);
        }

        /// <summary>
        /// 获得连接对象
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection GetSqLiteConnection()
        {
            return new SQLiteConnection(SqlConnctionString);
        }

        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, string cmdText, params object[] p)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (p != null)
            {
                foreach (object parm in p)
                    cmd.Parameters.AddWithValue(string.Empty, parm);
                //for (int i = 0; i < p.Length; i++)
                // cmd.Parameters[i].Value = p[i];
            }
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="cmdText">a</param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, params object[] p)
        {
            var command = new SQLiteCommand();
            using (var connection = GetSqLiteConnection())
            {
                PrepareCommand(command, connection, cmdText, p);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteReader(string cmdText, params object[] p)
        {
            var command = new SQLiteCommand();
            var connection = GetSqLiteConnection();
            try
            {
                PrepareCommand(command, connection, cmdText, p);
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters">传入的参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params object[] p)
        {
            var cmd = new SQLiteCommand();
            using (var connection = GetSqLiteConnection())
            {
                PrepareCommand(cmd, connection, cmdText, p);
                return cmd.ExecuteScalar();
            }
        }
    }
}