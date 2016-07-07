using HeatronicUwpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks
{
    public sealed class HeatronicListenerTask : IBackgroundTask, IDisposable
    {

        private BackgroundTaskDeferral _deferral;
        private AppServiceConnection appServiceconnection;

        public void Run(IBackgroundTaskInstance taskInstance)
        {

            var packageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;

            //keep this background task alive
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            // Start HeatronicGateway
            //HeatronicGateway heatronicGateway = HeatronicGateway.GetDefault();
            // Listen to new messages from the Heatronic Gateway
            //heatronicGateway.NewMessage += HeatronicGateway_NewMessage;

            // Retrieve the app service connection
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails; // ApplicationTriggerDetails;
            //details.CallerPackageFamilyName
            //            appServiceconnection = details.AppServiceConnection;
            //            appServiceconnection.RequestReceived += OnRequestReceived;
            //appServiceconnection.SendMessageAsync();

            

        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // Get a deferral because we use an awaitable API below to respond to the message
            // and we don't want this call to get cancelled while we are waiting.
            var messageDeferral = args.GetDeferral();

            ValueSet message = args.Request.Message;
            ValueSet returnData = new ValueSet();

            //string command = message["Command"] as string;

            returnData.Add("Result", "Value");


            // Return the data to the caller.
            await args.Request.SendResponseAsync(returnData); 
            messageDeferral.Complete(); 
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

        private void HeatronicGateway_NewMessage(object sender, NewMessageEventArgs e)
        {
            

        }


        /// <summary>
        /// Frees resources held by this background task.
        /// </summary>
        public void Dispose()
        {
            HeatronicGateway heatronicGateway = HeatronicGateway.GetDefault();
            if (null != heatronicGateway)
            {
                heatronicGateway.NewMessage -= HeatronicGateway_NewMessage;
            }
        }
    }
}
