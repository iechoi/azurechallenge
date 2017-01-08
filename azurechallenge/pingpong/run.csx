using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    var pingValue = data?.ping;
    var response = new
    {
        pong = pingValue
    };

    return req.CreateResponse(HttpStatusCode.OK, response);
}