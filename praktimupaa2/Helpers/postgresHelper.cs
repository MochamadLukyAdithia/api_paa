using Npgsql;

namespace praktimupaa2.Helpers
{
    public class postgresHelper
    {
        private readonly NpgsqlConnection _conn;
        private readonly string _connString;
    
        public postgresHelper(string connString)
        {
            _connString = connString;
            _conn = new NpgsqlConnection(_connString);
        }
        public NpgsqlCommand GetNpgsqlCommand(string query)
        {
            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(query, _conn);
            return cmd;
        }
        public void closeConnection()
        {
            _conn.Close();
        }


    }
}
