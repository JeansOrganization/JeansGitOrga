
using System.Data;

public class SqlHelper
{
    public static List<T> QueryData<T>(IDbConnection conn, FormattableString formattable) where T : new()
    {
        using var cmd = CreateCommand(conn, formattable);
        using var reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        return ConvertDataToList<T>(dt).ToList<T>();
    }

    static IDbCommand CreateCommand(IDbConnection conn, FormattableString formattable)
    {
        string sql = formattable.Format;
        using var cmd = conn.CreateCommand();
        for (int i = 0; i < formattable.ArgumentCount; i++)
        {
            sql = sql.Replace("{" + i + "}", "@p" + i);
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = "@p" + i;
            parameter.Value = formattable.GetArgument(i);
            cmd.Parameters.Add(parameter);
        }
        cmd.CommandText = sql;
        return cmd;
    }

    public static IEnumerable<T> ConvertDataToList<T>(DataTable dt) where T : new()
    {
        foreach (DataRow dr in dt.Rows)
        {
            T obj = new T();
            foreach (var p in obj.GetType().GetProperties())
            {
                p.SetValue(obj, Convert.ToString(dr[p.Name]));
            }
            yield return obj;
        }
    }
}