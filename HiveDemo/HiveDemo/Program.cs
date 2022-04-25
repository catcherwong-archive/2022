using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Odbc;

SqlMapper.SetTypeMap(typeof(QueryResult), new CustomPropertyTypeMap(
    typeof(QueryResult), (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
    prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));

var data = HiveHelper.QueryRawData();

foreach (var item in data)
{
    Console.WriteLine(item.Id + "\t\t" + item.Bucket);
}

public static class HiveHelper
{
    // dsn
    // private static string _dsn = "DSN=hivedsn;Schema=ods;UID=xxx;PWD=xxx;";
    // dsn-less
    private static string _dsn = "Driver=Hive;Host=xxx.xxx.xxx.xxx;Port=10000;Schema=ods;UID=xxx;PWD=xxx;";

    public static List<QueryResult> QueryRawData()
    {
        var sql = $" select id, bucket from ods_xxx_oss where dt = '2022-01-21' limit 10;  ";

        try
        {
            using var conn = new OdbcConnection(_dsn);
            conn.Open();
            var res = conn.Query<QueryResult>(sql, commandTimeout: 600000000);            
            return res.ToList();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

public class QueryResult
{
    [Column("ods_xxx_oss.id")]
    public string? Id { get; set; }

    [Column("ods_xxx_oss.bucket")]
    public string? Bucket { get; set; }
}
