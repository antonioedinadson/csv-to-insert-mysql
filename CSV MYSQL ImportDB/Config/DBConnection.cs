using MySql.Data.MySqlClient;

namespace CSV_MYSQL_ImportDB.Config
{
    internal class DBConnection
    {
        private string _connectionString;
        private readonly MySqlConnection _connection;

        public DBConnection(string server, string database, string username, string password)
        {
            _connectionString = $"server={server};database={database};uid={username};password={password};";
            _connection = new MySqlConnection(_connectionString);
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnetion()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnetion()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

    }
}
