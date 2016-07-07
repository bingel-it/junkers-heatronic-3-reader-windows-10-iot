using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            System.Threading.Tasks.Task.Run(async () =>
            {
                Temp();
            });

            
        }


        String taskName = "Heatronic Listener BackgroundTask";

        private async void Temp()
        {

            var packageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            var appServiceName = typeof(HeatronicUwpApp.Tasks.HeatronicListenerTask).FullName;
            var appServiceConnection = new Windows.ApplicationModel.AppService.AppServiceConnection()
            {
                PackageFamilyName = packageFamilyName,
                AppServiceName = "de.bingelit.myhome.heatronic"
            };


            var status = await appServiceConnection.OpenAsync();
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            if (status != Windows.ApplicationModel.AppService.AppServiceConnectionStatus.Success)
            {
               
                //return;
            }

            var message = new ValueSet();
            var response = await appServiceConnection.SendMessageAsync(message);
            if (response.Status == Windows.ApplicationModel.AppService.AppServiceResponseStatus.Success)
            {
                // Get the data  that the service sent  to us.
                if (response.Message["Result"] as string == "OK")
                {
                    var result = response.Message["Result"] as string;
                }
            }

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
    }
}
