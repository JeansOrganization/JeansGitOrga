
using System.Data;
using MySql.Data.MySqlClient;

var connStr = "server=127.0.0.1;Port=3306;user id=Jean;password=123456;database=jeandb";
using (var conn = new MySqlConnection(connStr))
{
    conn.Open();
    var command = conn.CreateCommand();
    command.CommandText = "select * from fa_bazi_order";

    DataTable dt = new DataTable();
    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
    {
        adapter.Fill(dt);

        List<string> colList = new List<string>();
        foreach (DataColumn dc in dt.Columns)
        {
            if (!(new string[] { "id", "user_id", "name", "createtime", "order_sn", "man_birthday" }).Contains(dc.ColumnName)) continue;
            colList.Add(dc.ColumnName);
        }

        foreach (DataRow dr in dt.Rows)
        {
            //Console.WriteLine($"id:{dr["id"]}, name:{dr["name"]}, price:{dr["price"]}" +
            //    $", days:{dr["days"]}, list_price:{dr["list_price"]}, createtime:{dr["createtime"]}");
            string outString = "";
            foreach (string colName in colList)
            {
                outString += $"{colName}:{dr[colName]},";
            }
            Console.WriteLine(outString.TrimEnd(','));
        }
        Console.ReadLine();
    }

    //var reader = command.ExecuteReader();
    //while (reader.Read())
    //{
    //    int id = reader.GetInt32(0);
    //    string name = reader.GetString(1);
    //    double price = reader.GetDouble(2);

    //    Console.WriteLine($"id:{id}, name:{name}, price:{price}");
    //}
    //Console.ReadLine();
    //reader.Close();
}