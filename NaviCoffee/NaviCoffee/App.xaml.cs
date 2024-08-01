using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NaviCoffee.Services;
using NaviCoffee.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Xamarin.Essentials;

namespace NaviCoffee
{
    public partial class App : Application
    {
        public static string AzureBackendUrl = 
           //DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
           "https://moorellc.us/";

        public App()
        {
            InitializeComponent();
            /*
                Services are components or class that encapsulates 
                sepcific busines sloigc which other parts of the 
                app rely on. They are modular, resuable, can be 
                Dependecy Injection, Loose coupling. 
            */

            DependencyService.Register<CoffeeService>();
            MainPage = new AppShell();
        }

        
        protected override void OnStart()
        {
            

            // Handle when your app starts            

            /*AppCenter.Start("ios=b6aa8342-8128-4e01-bfe3-50b7871f1ca8;" +
                  //+"uwp={Your UWP App secret here};" +
                  "android=971694d2-25c6-433b-b77a-fbd6929b4f8b;",
                  typeof(Analytics), typeof(Crashes));

            AppCenter.Start("ios=b6aa8342-8128-4e01-bfe3-50b7871f1ca8;android=971694d2-25c6-433b-b77a-fbd6929b4f8b", typeof(Distribute));

            Distribute.SetEnabledAsync(true);
            */
            //bool enabled = Distribute.IsEnabledAsync().Result;

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
