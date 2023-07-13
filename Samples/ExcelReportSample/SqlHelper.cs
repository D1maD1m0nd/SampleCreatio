using System.Data;
using System.Data.SqlClient;

namespace Terrasoft.Configuration.SummaryTableVisa;

public class SqlHelper
{
    private const string ConnectionString = "Data Source=D1MOND;Initial Catalog=valta7185;User ID=creatioadmin;Password=316853bl98d3Lbvf88801;";
    private SqlConnection Connection { get; set; }
    public async Task CreateConnection()
    {
        
        // Создание подключения
        Connection = new SqlConnection(ConnectionString);
        try
        {
            // Открываем подключение
            await Connection.OpenAsync();
            Console.WriteLine(Connection.ClientConnectionId);
            Console.WriteLine("Подключение открыто");
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task CreateTemporaryTable()
    {
        var query = @"CREATE TABLE #ProductSummary
                        (ProdId INT IDENTITY,
                        ProdName NVARCHAR(20),
                        Price MONEY)";
        SqlCommand command = new SqlCommand();
        command.CommandText = query;
        command.Connection = Connection;
        await command.ExecuteNonQueryAsync();
                 
        Console.WriteLine("Таблица ProductSummary создана"); 
    }

    public async Task InsertDataFromProductSummary()
    {
        var query = @"INSERT INTO #ProductSummary (ProdName, Price) VALUES ('Nokia 8', 18000), ('iPhone 8', 56000)";
        SqlCommand command = new SqlCommand();
        command.CommandText = query;
        command.Connection = Connection;
        await command.ExecuteNonQueryAsync();
    }
    public async Task SelectDataFromProductSummary()
    {
        var query = @"SELECT * FROM #ProductSummary";
        SqlCommand command = new SqlCommand();
        command.CommandText = query;
        command.Connection = Connection;
        SqlDataReader reader = await command.ExecuteReaderAsync();
        if (reader.HasRows) // если есть данные
        {
            // выводим названия столбцов
            string columnName1 = reader.GetName(0);
            string columnName2 = reader.GetName(1);
            string columnName3 = reader.GetName(2);
 
            Console.WriteLine($"{columnName1}\t{columnName3}\t{columnName2}");
 
            while (await reader.ReadAsync()) // построчно считываем данные
            {
                object id = reader.GetValue(0);
                object name = reader.GetValue(2);
                object age = reader.GetValue(1);
 
                Console.WriteLine($"{id} \t{name} \t{age}") ;
            }
        }
 
        await reader.CloseAsync();
    } 
    public static string GenerateCreateTableSql<T>(string userTableName = null, bool isTemporaryTable = false)
    {
        var tableName = string.IsNullOrEmpty(userTableName) ? typeof(T).Name : userTableName;
        var properties = typeof(T).GetProperties();
        var tempChar = isTemporaryTable ? "#" : string.Empty;
        var sql = $"CREATE TABLE {tempChar}{tableName} (";
        foreach (var property in properties)
        {
            var columnName = property.Name;
            var columnType = property.PropertyType.Name;
            sql += $"{columnName} {GetSqlType(columnType)}, ";
        }
        sql = sql.TrimEnd(',', ' ');
        sql += ")";
        return sql;
    }

    private static string GetSqlType(string dotNetType)
    {
        switch (dotNetType)
        {
            case "Int32":
                return "INT";
            case "String":
                return "NVARCHAR(MAX)";
            case "DateTime":
                return "DATETIME";
            default:
                return "NVARCHAR(MAX)";
        }
    }
}