using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism;
using Prism.Unity;
using Microsoft.Practices.Unity;

using Gcm.Client;

namespace Day4.Droid
{
	[Activity(Label = "Day4", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
        public static MainActivity CurrentActivity { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
            CurrentActivity = this;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			try
			{
				GcmClient.CheckDevice(this);
				GcmClient.CheckManifest(this);
				GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
			}
			catch (Exception ex)
	        {
                Console.WriteLine($"[=================================] {ex.ToString()}");
	        }

			LoadApplication(new App(new AndroidInitializer()));
		}
	}

	public class AndroidInitializer : IPlatformInitializer
	{
		public void RegisterTypes(IUnityContainer container)
		{

		}
	}
}
