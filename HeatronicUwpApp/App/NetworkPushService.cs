using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Extentions;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Rest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.App
{
    class NetworkPushService
    {
        private ConcurrentDictionary<String, Uri> listeners = new ConcurrentDictionary<string, Uri>();
        private HttpClient httpClient = null;

        public NetworkPushService()
        {
            httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.TryParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async void SendMessageToListenerAsync(String subUrl, BaseMessageRto message)
        {
            List<String> listenersToRemove = new List<string>();

            var httpResponse = new HttpResponseMessage();
            foreach (var listenerKey in this.listeners.Keys)
            {
                var listenerUrl = listeners[listenerKey];
                try
                {
                    //Send the GET request
                    httpResponse = await httpClient.PostAsJsonAsync(listenerUrl, message);
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
    }
}
