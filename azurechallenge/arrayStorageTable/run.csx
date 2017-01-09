#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static HttpResponseMessage Run(HttpRequestMessage req, CloudTable tableBinding, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = req.Content.ReadAsStringAsync().Result;
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    var key = data?.key?.Value;
    var arrayEntity = new ArrayEntity(key);
    arrayEntity.arrayString = data?.ArrayOfValues.ToString();

    tableBinding.Execute(TableOperation.InsertOrReplace(arrayEntity));

    return req.CreateResponse(HttpStatusCode.OK, new
    { });
}

public class ArrayEntity : TableEntity
{
    public ArrayEntity(string rowKey)
    {
        this.PartitionKey = "rofl";
        this.RowKey = rowKey;
    }

    public ArrayEntity() { }

    public string arrayString { get; set; }
}