using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Rest;
using BingelIT.MicroWebServerLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.App
{
    class HeatronicApp
    {
   
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

            System.Threading.Tasks.Task.Run(() =>
            {
                this.heatronicAppServiceClient.OpenAsync();
            });
       
        }


        private void AppServiceConnection_RequestReceived(Windows.ApplicationModel.AppService.AppServiceConnection sender, Windows.ApplicationModel.AppService.AppServiceRequestReceivedEventArgs args)
        {

        }
     
    }
}
