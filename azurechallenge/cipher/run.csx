using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    Guid? key = data?.key;
    string msg = data?.msg;
    Dictionary<string, int> cipher = data?.cipher.ToObject<Dictionary<string, int>>();

    StringBuilder result = new StringBuilder();
    foreach (string letterPair in msg.SplitByLength(2))
    {
        int intValue = int.Parse(letterPair);
        string letter = cipher.FirstOrDefault(kvp => kvp.Value == intValue).Key;
        result.Append(letter);
    }

    log.Info($"Secret message received! {result.ToString()}");

    var response = new
    {
        key = key,
        result = result.ToString()
    };

    return req.CreateResponse(HttpStatusCode.OK, response);
}

public static IEnumerable<string> SplitByLength(this string str, int maxLength)
{
    for (int index = 0; index < str.Length; index += maxLength)
    {
        yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
    }
}