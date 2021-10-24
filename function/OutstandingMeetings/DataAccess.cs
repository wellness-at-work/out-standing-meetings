using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace FnOutstandingMeetings
{
    public class DataAccess<T> where T : TableEntity, new()
    {
        private readonly CloudTable _tableClient;

        public DataAccess(CloudTable cloudTable)
        {
            _tableClient = cloudTable;
        }

        public virtual async Task InsertAsync(T item)
        {
            var operation = TableOperation.Insert(item);
            await _tableClient.ExecuteAsync(operation);
        }

        public virtual async Task<T> GetAsync(string partition, string key)
        {
            var operation = TableOperation.Retrieve<T>(partition, key);
            var result = await _tableClient.ExecuteAsync(operation);
            var item = result.Result as T;
            return item;
        }

        public virtual async Task ReplaceAsync(T item)
        {
            var operation = TableOperation.Replace(item);
            await _tableClient.ExecuteAsync(operation);
        }
    }
}
