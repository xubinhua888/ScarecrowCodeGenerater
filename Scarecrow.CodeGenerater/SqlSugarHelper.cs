using Mapster;

namespace Scarecrow.CodeGenerater;

internal class SqlSugarHelper
{
    private static SqlSugarClient GetSqlClient(GeneraterConfig config)
    {
        return new SqlSugarClient(new ConnectionConfig
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), config.DbType),
            ConnectionString = config.ConnectionString,
            IsAutoCloseConnection = true
        });
    }

    public static List<DbTableModel> GetTableList(GeneraterConfig config)
    {
        AnsiConsole.MarkupLine($"[lightcoral]开始查询table列表. . .[/]");
        var res = new List<DbTableModel>();
        var client = GetSqlClient(config);
        var tables = client.DbMaintenance.GetTableInfoList(false);
        if (tables == null || tables.Count < 1)
        {
            return res;
        }
        var tableNames = new List<string>();
        if (config.TableNames.IsNotNullAndWhiteSpace())
        {
            tableNames = config.TableNames.Replace("，", ",").Replace(" ", ",").Split(',').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        }
        if (config.IgnoreTablePrefix == null) config.IgnoreTablePrefix = new();
        if (config.IgnoreColumns == null) config.IgnoreColumns = new();
        foreach (var table in tables)
        {
            if (table.DbObjectType != DbObjectType.Table)
            {
                continue;
            }
            if (tableNames.Count > 0 && !tableNames.Contains(table.Name, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }
            if (config.IgnoreTablePrefix.Count > 0 && config.IgnoreTablePrefix.Exists(a => table.Name.StartsWith(a, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }
            res.Add(table.Adapt<DbTableModel>());
        }
        if (res.Count < 1)
        {
            return res;
        }
        AnsiConsole.MarkupLine($"[lightcoral]开始查询表字段信息. . .[/]");
        var typeMap = GetTypeMap(config);
        foreach (var table in res)
        {
            table.Init();
            var columns = client.DbMaintenance.GetColumnInfosByTableName(table.Name, false);
            table.Columns = columns.Adapt<List<DbColumnModel>>();
            foreach (var column in table.Columns)
            {
                if (config.IgnoreColumns.Exists(a => string.Equals(a, column.DbColumnName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                if (typeMap.ContainsKey(column.DataType))
                {
                    if (string.Equals("ignore", typeMap[column.DataType], StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    column.CSharpTypeName = typeMap[column.DataType];
                }
                else
                {
                    column.CSharpTypeName = client.Ado.DbBind.GetPropertyTypeName(column.DataType);
                }
                column.Init();
            }
            table.Columns = table.Columns.Where(a => a.CSharpTypeName.IsNotNullAndWhiteSpace()).ToList();
        }
        return res;
    }

    private static Dictionary<string, string> GetTypeMap(GeneraterConfig config)
    {
        if (config.TypeMapFile.IsNullOrWhiteSpace() || !File.Exists(config.TypeMapFile))
        {
            return new Dictionary<string, string>();
        }
        try
        {
            var mapJson = File.ReadAllText(config.TypeMapFile, System.Text.Encoding.UTF8);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapJson);
            return new Dictionary<string, string>(dic, StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return new Dictionary<string, string>();
        }
    }
}
