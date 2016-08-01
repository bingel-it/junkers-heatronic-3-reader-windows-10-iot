using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Data.Json;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients
{
    partial class AppServiceClient
    {

        public delegate void NewMessageEventHandler(object sender, NewMessageEventArgs e);
        public event NewMessageEventHandler NewMessage;

        public AppServiceClient() {
        }

        public async void OpenAsync()
        {
            // https://msdn.microsoft.com/de-de/windows/uwp/launch-resume/how-to-create-and-consume-an-app-service

            var packageFamilyName = Package.Current.Id.FamilyName;
            var appServiceName = "de.bingelit.myhome.heatronic.bgTask";
            var appServiceConnection = new AppServiceConnection()
            {
                PackageFamilyName = packageFamilyName,
                AppServiceName = appServiceName
            };

            var status = await appServiceConnection.OpenAsync();
            if (status != Windows.ApplicationModel.AppService.AppServiceConnectionStatus.Success)
            {
                throw new Exception("Could not connect to Heatronic application service");
            }

            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
        }

        private void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            ProcessRequestFromHeatronic(args);
        }

    }
}
