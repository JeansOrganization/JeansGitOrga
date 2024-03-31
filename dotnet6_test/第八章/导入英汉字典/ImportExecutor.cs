using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace 导入英汉字典
{
    public class ImportExecutor
    {
        private readonly IHubContext<ImportDictHub> hubContext;
        private readonly IOptionsSnapshot<ConnStrOptions> optionsSnapshot;

        public ImportExecutor(IHubContext<ImportDictHub> hubContext, IOptionsSnapshot<ConnStrOptions> optionsSnapshot)
        {
            this.hubContext = hubContext;
            this.optionsSnapshot = optionsSnapshot;
        }

        public async Task ExecuteAsync(string connectionId)
        {
            string[] segments = File.ReadAllLines(System.Environment.CurrentDirectory + "/stardict.csv");
            var client = hubContext.Clients.Client(connectionId);
            string connStr = optionsSnapshot.Value.Default;

            using SqlConnection conn = new SqlConnection(connStr);
            await conn.OpenAsync();
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conn);
            sqlBulk.DestinationTableName = "T_WordItems";
            sqlBulk.ColumnMappings.Add("Word", "Word");
            sqlBulk.ColumnMappings.Add("Phonetic","Phonetic");
            sqlBulk.ColumnMappings.Add("Definition","Definition");
            sqlBulk.ColumnMappings.Add("Translation","Translation");
            using DataTable dt = new DataTable();
            dt.Columns.Add("Word");
            dt.Columns.Add("Phonetic");
            dt.Columns.Add("Definition");
            dt.Columns.Add("Translation");
            
            int count = segments.Length;
            for(int i = 1; i < count; i++)
            {
                string lineStr = segments[i];
                string[] lineArr = lineStr.Split(',');
                var dr = dt.NewRow();
                dr["Word"] = lineArr[0];
                dr["Phonetic"] = lineArr[1];
                dr["Definition"] = lineArr[2];
                dr["Translation"] = lineArr[3];
                dt.Rows.Add(dr);
                if (dt.Rows.Count >= 1000)
                {

                    await sqlBulk.WriteToServerAsync(dt);
                    dt.Clear();
                    await client.SendAsync("ImportProgress", i, count);
                }
            }

            await sqlBulk.WriteToServerAsync(dt);
            dt.Clear();
            await client.SendAsync("ImportProgress", count, count);
        }
    }
}
