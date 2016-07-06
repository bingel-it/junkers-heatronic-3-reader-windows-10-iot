using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Rest;
using BingelIT.MyHome.MicroWebServerLib;
using HeatronicUwpLib;
using HeatronicUwpLib.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BingelIT.MyHome.MicroWebServerLib.HttpRequest;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.App
{
    class Class1
    {
        private WebServer webServer = null;
        private HeatronicGateway heatronicGateway = null;
        private NetworkPushService networkPushService = null;

        public void StartApp()
        {

            try
            {
                this.webServer = new WebServer(91);
                webServer.AddPathHandler("/rest/time/", HttpRequestMethod.Get, getDateTimeHandler); // Aktuelle Uhrzeit Datum
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Put, getNetworkPushInitHandler);
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Delete, getNetworkPushInitDeleteHandler);

                this.heatronicGateway = new HeatronicGateway();

                // Start Webserver
                webServer.InitAsync();

                // Start Heatronic Gateway
                this.heatronicGateway.InitAsync();

                // Network Push Service
                networkPushService = new NetworkPushService();
                this.heatronicGateway.NewMessage += HeatronicGateway_NewMessage;

            }
            catch (Exception ex)
            {
                Debug.Fail("Startup Failed " + ex.Message);
            }

        }

        private HttpResponse getNetworkPushInitDeleteHandler(HttpRequest request)
        {
            var key = request.Arguments["key"];
            networkPushService.RemoveListener(key);
            return HttpResponseBuilder.CreateOKResponse();
        }

        private void HeatronicGateway_NewMessage(object sender, NewMessageEventArgs e)
        {
            switch (e.MessageType)
            {
                case MessageType.Heater:
                    {
                        var msg = e.Message as HeizgeraetDTO;
                        var restMsg = new HeizgeraetMessageRto();
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.BetriebsflammeAn = msg.BetriebsflammeAn;
                        restMsg.BrennerLeistung = msg.BrennerLeistung;
                        restMsg.HeizungsMode = msg.HeizungsMode;
                        restMsg.HeizungsPumpeAn = msg.HeizungsPumpeAn;
                        restMsg.ZirkulationsPumpeAn = msg.ZirkulationsPumpeAn;

                        networkPushService.SendMessageToListenerAsync("/heater", restMsg);

                    }
                    break;
                case MessageType.Timestamp:
                    {
                        var msg = e.Message as TimestampDTO;
                        var restMsg = new TimestampMessageRto();
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.SystemTimestamp = msg.SystemTimestamp;

                        networkPushService.SendMessageToListenerAsync("/systemTime", restMsg);
                    }
                    break;
                case MessageType.HeaterCircuit:
                    {
                        var msg = e.Message as HeizkreisDTO;
                        var restMsg = new HeizkreisMessageRto();
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.Aussentemperatur = msg.Aussentemperatur;
                        networkPushService.SendMessageToListenerAsync("/heaterCircuit", restMsg);
                    }
                    break;
                case MessageType.Warmwater:
                    {
                        var msg = e.Message as WarmWaterDTO;
                        var restMsg = new WarmwasserMessageRto();
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.IstTemperatur = msg.IstTemperatur;
                        restMsg.SollTemperatur = msg.SollTemperatur;

                        networkPushService.SendMessageToListenerAsync("/warmwasser", restMsg);
                    }
                    break;
                default:
                    break;
            }
        }


        private HttpResponse getNetworkPushInitHandler(HttpRequest request)
        {
            var key = request.Arguments["key"];
            var uri = new Uri(request.Arguments["uri"]);
            networkPushService.Init(key, uri);

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
    }
}
