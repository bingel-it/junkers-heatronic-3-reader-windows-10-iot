using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpAppTestClient
{
    static class HttpClientExtention
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, Uri uri, T item)
        {
            var itemAsJson = JSONSerializer.SerializeObject(item);
            var content = new StringContent(itemAsJson, System.Text.Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(uri, content);
        }
    }
}
