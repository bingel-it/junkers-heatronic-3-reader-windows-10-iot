using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto;
using BingelIT.MyHome.Heatronic.HeatronicUwpApp.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients
{
    partial class AppServiceClient
    {

        private void ProcessRequestFromHeatronic(AppServiceRequestReceivedEventArgs args)
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
            Debug.WriteLine(DateTime.Now + ": New message: " + e.MessageType + " - " + e.Message.ToString());
            if (NewMessage != null)
                NewMessage(this, e);
        }
    }
}
