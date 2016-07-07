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

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic
{
    class HeatronicAppServiceClient
    {

        public delegate void NewMessageEventHandler(object sender, NewMessageEventArgs e);
        public event NewMessageEventHandler NewMessage;

        public HeatronicAppServiceClient() {
 
        }

        public async void OpenAsync()
        {
            // https://msdn.microsoft.com/de-de/windows/uwp/launch-resume/how-to-create-and-consume-an-app-service

            var packageFamilyName = Package.Current.Id.FamilyName;
            var appServiceName = "de.bingelit.myhome.heatronic";
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
            var messageTypeString = args.Request.Message["MessageType"] as string;
            var messageString = args.Request.Message["Message"] as string;

            var messageType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), messageTypeString, true);
            HeatronicDTO message = null;
            switch (messageType)
            {
                case MessageTypeEnum.Timestamp:
                    message = JSONSerializer.DeSerialize<TimestampDTO>(messageString);
                    break;
                case MessageTypeEnum.Heater:
                    message = JSONSerializer.DeSerialize<HeizgeraetDTO>(messageString);
                    break;
                case MessageTypeEnum.HeaterCircuit:
                    message = JSONSerializer.DeSerialize<HeizkreisDTO>(messageString);
                    break;
                case MessageTypeEnum.Warmwater:
                    message = JSONSerializer.DeSerialize<WarmWaterDTO>(messageString);
                    break;
                default:
                    break;
            }
            OnNewMessage(new NewMessageEventArgs()
            {
                MessageType = messageType,
                Message = message
            });

        }

        protected virtual void OnNewMessage(NewMessageEventArgs e)
        {
            Debug.WriteLine(DateTime.Now + ": New message: " + e.MessageType);
            if (NewMessage != null)
                NewMessage(this, e);
        }
    }
}
