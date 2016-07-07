using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Rest;
using BingelIT.MyHome.MicroWebServerLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using static BingelIT.MyHome.MicroWebServerLib.HttpRequest;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.App
{
    class HeatronicApp
    {
        //        private WebServer webServer = null;
        //        private NetworkPushService networkPushService = null;
        private static HeatronicApp _instance;

        private HeatronicApp()
        {

        }

        internal static HeatronicApp GetDefault()
        {
            if (_instance == null)
            {
                _instance = new HeatronicApp();
            }
            return _instance;
        }

        private HeatronicAppServiceClient heatronicAppServiceClient;

        public void StartApp()
        {

            this.heatronicAppServiceClient = new HeatronicAppServiceClient();

            System.Threading.Tasks.Task.Run(async () =>
            {
                this.heatronicAppServiceClient.OpenAsync();
            });
            

            //var message = new ValueSet();
            //var response = await appServiceConnection.SendMessageAsync(message);
            //if (response.Status == Windows.ApplicationModel.AppService.AppServiceResponseStatus.Success)
            //{
            //    // Get the data  that the service sent  to us.
            //    if (response.Message["Result"] as string == "OK")
            //    {
            //        var result = response.Message["Result"] as string;
            //    }
            //}

            //try
            //{
            //    Windows.ApplicationModel.Background.ApplicationTrigger trigger = null;

            //    if (!Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks.Any(reg => reg.Value.Name == taskName))
            //    {
            //        trigger = new Windows.ApplicationModel.Background.ApplicationTrigger();
            //        //erstellen und registrieren 
            //        var builder = new Windows.ApplicationModel.Background.BackgroundTaskBuilder();

            //        builder.Name = taskName;
            //        builder.TaskEntryPoint = typeof(HeatronicUwpApp.Tasks.HeatronicListenerTask).FullName;
            //        builder.SetTrigger(trigger);

            //        builder.Register();
            //    }
            //    else
            //    {
            //        var registration = Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks.FirstOrDefault(reg => reg.Value.Name == taskName).Value as Windows.ApplicationModel.Background.BackgroundTaskRegistration;
            //        trigger = registration.Trigger as Windows.ApplicationModel.Background.ApplicationTrigger;

            //    }

            //    var taskParameters = new ValueSet();
            //    var taskResult = await trigger.RequestAsync(taskParameters);


            //}
            //catch (Exception ex)
            //{

            //}
        }


        private void AppServiceConnection_RequestReceived(Windows.ApplicationModel.AppService.AppServiceConnection sender, Windows.ApplicationModel.AppService.AppServiceRequestReceivedEventArgs args)
        {

        }

        //try
        //{
        //    this.webServer = new WebServer(91);
        //    webServer.AddPathHandler("/rest/time/", HttpRequestMethod.Get, getDateTimeHandler); // Aktuelle Uhrzeit Datum
        //    webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Put, getNetworkPushInitHandler);
        //    webServer.AddPathHandler("/rest/init/*", HttpRequestMethod.Delete, getNetworkPushInitDeleteHandler);

        //    this.heatronicGateway = new HeatronicGateway();

        //    // Start Webserver
        //    webServer.InitAsync();

        //    // Start Heatronic Gateway
        //    this.heatronicGateway.InitAsync();

        //    // Network Push Service
        //    networkPushService = new NetworkPushService();
        //    this.heatronicGateway.NewMessage += HeatronicGateway_NewMessage;

        //}
        //catch (Exception ex)
        //{
        //    Debug.Fail("Startup Failed " + ex.Message);
        //}


        //private HttpResponse getNetworkPushInitDeleteHandler(HttpRequest request)
        //{
        //    var key = request.Arguments["key"];
        //    networkPushService.RemoveListener(key);
        //    return HttpResponseBuilder.CreateOKResponse();
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


        //private HttpResponse getNetworkPushInitHandler(HttpRequest request)
        //{
        //    var key = request.Arguments["key"];
        //    var uri = new Uri(request.Arguments["uri"]);
        //    networkPushService.Init(key, uri);

        //    return HttpResponseBuilder.CreateOKResponse();
        //}

        //private HttpResponse getDateTimeHandler(HttpRequest request)
        //{
        //    var httpResponse = HttpResponseBuilder.CreateOKResponse(
        //        "{ \"UTC\": \"" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\" }",
        //        "application/json"
        //    );
        //    return httpResponse;
        //}
    }
}
