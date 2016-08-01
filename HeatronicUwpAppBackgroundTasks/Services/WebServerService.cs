using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Helper;
using BingelIT.MicroWebServerLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel;
using HeatronicUwpLib;
using HeatronicUwpLib.Dto;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Rto;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Services
{
    public class WebServerService
    {
        private WebServer webServer;

        private NetworkPushService networkPushService;

        private RtoFactory rtoFactory = new RtoFactory();

        private static WebServerService _instance;

        public static WebServerService GetDefault()
        {
            if (_instance == null)
            {
                _instance = new WebServerService();
            }
            return _instance;
        }

        private WebServerService() { }


        public void Init()
        {

            try
            {
                this.webServer = new WebServer(91);
                webServer.AddPathHandler("/rest/time/", HttpRequestMethod.Get, getDateTimeHandler); // Aktuelle Uhrzeit Datum
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Put, getNetworkPushInitHandler);
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Delete, getNetworkPushInitDeleteHandler);

                // Start Webserver
                webServer.InitAsync();

                // Init NetworkPushService
                networkPushService = new NetworkPushService();

                // Init Heatronic
                HeatronicGateway heatronicGateway = HeatronicGateway.GetDefault();
                // Listen to new messages from the Heatronic Gateway
                heatronicGateway.NewMessage += HeatronicGateway_NewMessageForWebService;

            }
            catch (Exception ex)
            {
                Debug.Fail("Failed to initialize the WebServer: " + ex.Message);
                throw ex;
            }
        }


        private void HeatronicGateway_NewMessageForWebService(object sender, NewMessageEventArgs e)
        {
            try
            {
                var restMsg = rtoFactory.ConvertMessage(e.MessageType, e.Message);
                var restStrMsg = JSONSerializer.SerializeObject(restMsg);
                switch (e.MessageType)
                {
                    case MessageType.Heater:
                        networkPushService.SendJsonToListnerAsync("/heater", restStrMsg);
                        break;
                    case MessageType.Timestamp:
                        networkPushService.SendJsonToListnerAsync("/timestamp", restStrMsg);
                        break;
                    case MessageType.HeaterCircuit:
                        networkPushService.SendJsonToListnerAsync("/heaterCircuit", restStrMsg);
                        break;
                    case MessageType.Warmwater:
                        networkPushService.SendJsonToListnerAsync("/warmwater", restStrMsg);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception while sending Json to server: " + ex.Message);
                return;
            }

        }

        private HttpResponse getNetworkPushInitHandler(HttpRequest request)
        {
            var key = request.Arguments["key"];
            var uri = new Uri(request.Arguments["uri"]);
            networkPushService.Init(key, uri);

            return HttpResponseBuilder.CreateOKResponse();
        }

        private HttpResponse getNetworkPushInitDeleteHandler(HttpRequest request)
        {
            var key = request.Arguments["key"];
            networkPushService.RemoveListener(key);
            return HttpResponseBuilder.CreateOKResponse();
        }

        private HttpResponse getDateTimeHandler(HttpRequest request)
        {
            var httpResponse = HttpResponseBuilder.CreateOKResponse(
                "{ \"UTC\": \"" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\" }",
                "application/json"
            );
            return httpResponse;
        }

        /// <summary>
        /// Frees resources held by this background task.
        /// </summary>
        public void DisposeWebServer()
        {
            if (this.webServer != null)
            {
                webServer.Stop();
                webServer.Dispose();
            }

            if (this.networkPushService != null)
            {
                this.networkPushService.Dispose();
            }
        }
    }
}
