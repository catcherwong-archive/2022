using System.Data.Common;

namespace Common
{
    public class Db
    {
        private static readonly string _conn = "Data Source=127.0.0.1, 1433;Initial Catalog=test;user id=dev;password=123456;TrustServerCertificate=True;";

        public static DbConnection GeConn() => new Microsoft.Data.SqlClient.SqlConnection(_conn);
    }
}