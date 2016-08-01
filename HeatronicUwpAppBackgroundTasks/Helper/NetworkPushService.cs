using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Helper
{
    class NetworkPushService : IDisposable
    {
        private ConcurrentDictionary<String, Uri> listeners = new ConcurrentDictionary<string, Uri>();
        private HttpClient httpClient = null;

        public NetworkPushService()
        {
            httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.TryParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async void SendJsonToListnerAsync(String subUrl, String jsonString)
        {
            List<String> listenersToRemove = new List<string>();

            var httpResponse = new HttpResponseMessage();
            foreach (var listenerKey in this.listeners.Keys)
            {
                var listenerUrl = listeners[listenerKey];
                try
                {
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                    //Send the GET request
                    httpResponse = await httpClient.PostAsync(listenerUrl, content);
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        listenersToRemove.Add(listenerKey);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed sending Message to " + ex.Message, ex.HResult.ToString("X"));
                }
            }

            foreach (var key in listenersToRemove)
            {
                Uri uri;
                listeners.TryRemove(key, out uri);
            }

        }

        public void Init(String listenerkey, Uri listenerUri)
        {
            this.listeners.TryAdd(listenerkey, listenerUri);
        }

        internal void RemoveListener(string key)
        {
            Uri value; 
            this.listeners.TryRemove(key, out value);
        }

        public void Dispose()
        {
            this.listeners.Clear();

            if (this.httpClient != null)
            {
                this.httpClient.Dispose();
            }
        }
    }
}
