using HeatronicUwpLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks
{
    /// <summary>
    /// Heatronic listener background task. Listening to the Heatronic bus and sends the messages to all application clients
    /// </summary>
    public sealed class HeatronicListenerTask : IBackgroundTask, IDisposable
    {

        /// <summary>
        /// Task deferral for async processing 
        /// </summary>
        private BackgroundTaskDeferral _deferral;

        /// <summary>
        /// Application service connection to communicate between this task and the client
        /// </summary>
        private AppServiceConnection applicationServiceConnection;


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
                // Start HeatronicGateway
                HeatronicGateway heatronicGateway = HeatronicGateway.GetDefault();
                // Listen to new messages from the Heatronic Gateway
                heatronicGateway.NewMessage += HeatronicGateway_NewMessage;
            } catch (Exception ex)
            {
                Debug.Fail("Failed to initialize the HeatronicGateway: " + ex.Message);
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
        private async void HeatronicGateway_NewMessage(object sender, NewMessageEventArgs e)
        {
            var message = new ValueSet();
            message.Add("MessageType", e.MessageType);
            message.Add("Message", e.Message); // TODO: Send as Json?

            // Send Message to all client
            await applicationServiceConnection.SendMessageAsync(message);
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
