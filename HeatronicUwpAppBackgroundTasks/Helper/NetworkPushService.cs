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
        private ConcurrentDictionary<String, MessageListener> listeners = new ConcurrentDictionary<string, MessageListener>();
        private HttpClient httpClient = null;

        public NetworkPushService()
        {
            httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.TryParseAdd("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async void SendJsonToListnerAsync(String subUrl, String jsonString)
        {
            Debug.WriteLine("Sending: " + jsonString);
            List<String> listenersToRemove = new List<string>();

            using (var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json"))
            {
                foreach (var listenerKey in this.listeners.Keys)
                {
                    var listener = listeners[listenerKey];
                    try
                    {
                        //Send the GET request
                        using (var httpResponse = await httpClient.PostAsync(listener.Uri, content))
                        {
                            Debug.WriteLine("Sending Message to server");
                            if (!httpResponse.IsSuccessStatusCode)
                            {
                                listener.TryCounter = 0;
                                listenersToRemove.Add(listenerKey);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed sending Message to " + ex.Message, ex.HResult.ToString("X"));
                        listener.TryCounter++;
                        if (listener.TryCounter > 10)
                        {
                            listenersToRemove.Add(listenerKey);
                        }
                    }
                }
            }


            foreach (var key in listenersToRemove)
            {
                MessageListener uri;
                listeners.TryRemove(key, out uri);
                Debug.WriteLine("Removing listener: " + key);
            }

        }

        public void Init(String listenerkey, Uri listenerUri)
        {
            var listener = new MessageListener()
            {
                Uri = listenerUri
            };

            this.listeners.TryAdd(listenerkey, listener);
        }

        internal void RemoveListener(string key)
        {

            var listener = new MessageListener()
            {
                
            }; 
            this.listeners.TryRemove(key, out listener);
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

    class MessageListener
    {
        public Uri Uri { get; set; }

        public int TryCounter { get; set; }
    }
}
