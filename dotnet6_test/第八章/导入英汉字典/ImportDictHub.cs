using Microsoft.AspNetCore.SignalR;

namespace 导入英汉字典
{
    public class ImportDictHub : Hub
    {
        private readonly ImportExecutor importExecutor;

        public ImportDictHub(ImportExecutor importExecutor)
        {
            this.importExecutor = importExecutor;
        }

        public async Task Import()
        {
            await importExecutor.ExecuteAsync(this.Context.ConnectionId);
            //return Task.CompletedTask;
        }
    }
}
