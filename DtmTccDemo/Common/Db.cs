using MySqlConnector;
using System.Data.Common;

namespace Common
{
    public class Db
    {
        private static readonly string _conn = "Server=localhost;port=3306;User ID=root;Password=123456;Database=dtm_barrier";

        public static DbConnection GeConn() => new MySqlConnection(_conn);
    }
}