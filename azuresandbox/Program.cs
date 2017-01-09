using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace azuresandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteToTable("roflsup", new byte[] { 1, 2, 5, 3, 4, 10, 9, 6, 8, 7 });
        }

        public static void WriteToTable(string key, byte[] array)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=azurefunctionsc99a8a83;AccountKey=p28+NTD+38TscKfSbcbxzXfZkpk+HBQ5oWHnjKHb2G7mQEHJIQTIuAeNw32PkuCByWAGfUOzE3cIH5s9Orncxw==");
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("AzureChallengeTable");

            ArrayEntity arrayEntity = new ArrayEntity(key);
            arrayEntity.theArray = array;

            table.Execute(TableOperation.InsertOrReplace(arrayEntity));

            // Read the item
            var result = table.Execute(TableOperation.Retrieve<ArrayEntity>("rofl", key));
            var echoedItem = result.Result as ArrayEntity;

            foreach (var element in echoedItem.theArray.OrderBy(element => element))
            {
                Console.Write(element + " ");
            }

            dynamic data = JsonConvert.DeserializeObject("foo");

            var mykey = data?.key?.Value;
            var myarrayEntity = new ArrayEntity(key);
            
            arrayEntity.theArray = data?.ArrayOfValues?.Value;
        }
    }

    public class ArrayEntity : TableEntity
    {
        public ArrayEntity(string rowKey)
        {
            this.PartitionKey = "rofl";
            this.RowKey = rowKey;
        }

        public ArrayEntity() { }

        public byte[] theArray { get; set; }
    }
}
