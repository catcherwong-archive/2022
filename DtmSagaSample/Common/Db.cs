using MySqlConnector;
using System.Data.Common;
using Dapper;

namespace Common
{
    public class Db
    {
        private static readonly string _conn = "Server=localhost;port=3306;User ID=root;Password=123456;Database=dtm_barrier";

        public static DbConnection GeConn() => new MySqlConnection(_conn);

        public static async Task<bool> AdjustBalance(string userId, int amount)
        {
            try
            {
                var sql = "update dtm_busi.user_account set balance = balance + @amount where user_id = @userId";
                using var conn = GeConn();
                await conn.OpenAsync();
                var affectedRows = await conn.ExecuteAsync(sql, new { userId, amount });
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> BarrierAdjustBalance(string userId, int amount, DbTransaction tran)
        {
            try
            {
                var sql = "update dtm_busi.user_account set balance = balance + @amount where user_id = @userId";
                using var conn = tran.Connection;
                var affectedRows = await conn.ExecuteAsync(sql, new { userId, amount }, tran);
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}