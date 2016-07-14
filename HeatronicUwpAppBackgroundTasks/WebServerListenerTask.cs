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

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks
{
    public sealed class WebServerListenerTask : IBackgroundTask, IDisposable
    {
        /// <summary>
        /// Task deferral for async processing 
        /// </summary>
        private BackgroundTaskDeferral _deferral;

        /// <summary>
        /// Application service connection to communicate between this task and the client
        /// </summary>
        private AppServiceConnection applicationServiceConnection;

        private WebServer webServer;

        private NetworkPushService networkPushService;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //keep this background task alive
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            // Retrieve the app service connection
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            applicationServiceConnection = triggerDetails.AppServiceConnection;

            try
            {
                this.webServer = new WebServer(91);
                webServer.AddPathHandler("/rest/time/", HttpRequestMethod.Get, getDateTimeHandler); // Aktuelle Uhrzeit Datum
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Put, getNetworkPushInitHandler);
                webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Delete, getNetworkPushInitDeleteHandler);

                // Start Webserver
                webServer.InitAsync();

                // Init NetworkPushService
                networkPushService = new NetworkPushService(); //TODO: Refactor?
                //this.heatronicGateway.NewMessage += HeatronicGateway_NewMessage;

            }
            catch (Exception ex)
            {
                Debug.Fail("Failed to initialize the WebServer: " + ex.Message);
                if (_deferral != null)
                {
                    _deferral.Complete();
                }
            }

        }
        /// <summary> 
        /// Called when the background task is canceled by the app or by the system.
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_deferral != null)
            {
                _deferral.Complete();
            }
        }

        /// <summary>
        /// Process messages from the Heatronic bus and sends it to application service clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void HeatronicGateway_NewMessage(object sender, NewMessageEventArgs e)
        //{
        //    //var message = new ValueSet();
        //    //message.Add("MessageType", e.MessageType.ToString());
        //    //message.Add("Message", JSONSerializer.SerializeObject(e.Message));

        //    //// Send Message to all client
        //    //await applicationServiceConnection.SendMessageAsync(message);



        //    networkPushService.SendJsonToListnerAsync(subUrl, JSONSerializer.SerializeObject(item));

        //}

        //private void HeatronicGateway_NewMessage(object sender, NewMessageEventArgs e)
        //{
        //    switch (e.MessageType)
        //    {
        //        case MessageType.Heater:
        //            {
        //                var msg = e.Message as HeizgeraetDTO;
        //                var restMsg = new HeizgeraetMessageRto();
        //                restMsg.Timestamp = DateTime.Now;
        //                restMsg.BetriebsflammeAn = msg.BetriebsflammeAn;
        //                restMsg.BrennerLeistung = msg.BrennerLeistung;
        //                restMsg.HeizungsMode = msg.HeizungsMode;
        //                restMsg.HeizungsPumpeAn = msg.HeizungsPumpeAn;
        //                restMsg.ZirkulationsPumpeAn = msg.ZirkulationsPumpeAn;

        //                networkPushService.SendMessageToListenerAsync("/heater", restMsg);

        //            }
        //            break;
        //        case MessageType.Timestamp:
        //            {
        //                var msg = e.Message as TimestampDTO;
        //                var restMsg = new TimestampMessageRto();
        //                restMsg.Timestamp = DateTime.Now;
        //                restMsg.SystemTimestamp = msg.SystemTimestamp;

        //                networkPushService.SendMessageToListenerAsync("/systemTime", restMsg);
        //            }
        //            break;
        //        case MessageType.HeaterCircuit:
        //            {
        //                var msg = e.Message as HeizkreisDTO;
        //                var restMsg = new HeizkreisMessageRto();
        //                restMsg.Timestamp = DateTime.Now;
        //                restMsg.Aussentemperatur = msg.Aussentemperatur;
        //                networkPushService.SendMessageToListenerAsync("/heaterCircuit", restMsg);
        //            }
        //            break;
        //        case MessageType.Warmwater:
        //            {
        //                var msg = e.Message as WarmWaterDTO;
        //                var restMsg = new WarmwasserMessageRto();
        //                restMsg.Timestamp = DateTime.Now;
        //                restMsg.IstTemperatur = msg.IstTemperatur;
        //                restMsg.SollTemperatur = msg.SollTemperatur;

        //                networkPushService.SendMessageToListenerAsync("/warmwasser", restMsg);
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}



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
        public void Dispose()
        {
            if (null != this.webServer)
            {
                webServer.Stop();
                webServer.Dispose();
            }
        }
    }
}
