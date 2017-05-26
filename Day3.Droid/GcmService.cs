using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Util;
using Day4.Services;
using Gcm.Client;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Android.Support.V7.App;
using Android.Media;

namespace Day4.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
	public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
	{
		public static string[] SENDER_IDS = new string[] { "770315798703" };
	}

    [Service]
    public class GcmService : GcmServiceBase
	{

        string RegistrationID = "";

		public GcmService() : base(PushHandlerBroadcastReceiver.SENDER_IDS) { }

        protected override void OnRegistered(Context context, string registrationId)
		{
			Log.Verbose("PushHandlerBroadcastReceiver", "GCM Registered: " + registrationId);
			RegistrationID = registrationId;

            var push = AzureService.DefaultManager.CurrentClient.GetPush();

			MainActivity.CurrentActivity.RunOnUiThread(() => Register(push, null));
		}

		public async void Register(Microsoft.WindowsAzure.MobileServices.Push push, IEnumerable<string> tags)
		{
			try
			{
				const string templateBodyGCM = "{\"data\":{\"message\":\"$(messageParam)\"}}";

				JObject templates = new JObject();
				templates["genericMessage"] = new JObject
				{
					{"body", templateBodyGCM}
				};

				await push.RegisterAsync(RegistrationID, templates);
                Console.WriteLine($"[=================================] Push Installation Id {push.InstallationId.ToString()}");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				Debugger.Break();
			}
		}

		protected override void OnUnRegistered(Context context, string registrationId)
		{
            Console.WriteLine($"[=================================] PushHandlerBroadcastReceiver. Unregistered RegisterationToken: {registrationId}");
		}

		protected override void OnMessage(Context context, Intent intent)
		{
            //Push Notification arrived - print out the keys/values
            //if (intent == null || intent.Extras == null)
            //foreach (var key in intent.Extras.KeySet()){
            //    Console.WriteLine($"[=================================] Key: {key}, Value: {intent.Extras.GetString(key)}");
            //}

            var msg = $"New Customer: {intent.Extras.GetString("message")}";
            createNotification("Alert!", msg);
		}

		protected override bool OnRecoverableError(Context context, string errorId)
		{
            return false;
		}

		protected override void OnError(Context context, string errorId)
		{
            Console.WriteLine($"[=================================] PushHandlerBroadcastReceiver. GCM Error: {errorId}");
		}


		void createNotification(string title, string desc)
		{
		    //Create notification
		    var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

		    //Create an intent to show ui
		    var uiIntent = new Intent(this, typeof(MainActivity));

		    //Use Notification Builder
		    NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

		    //Create the notification
		    //we use the pending intent, passing our ui intent over which will get called
		    //when the notification is tapped.
		    var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
		            .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
		            .SetTicker(title)
		            .SetContentTitle(title)
		            .SetContentText(desc)

		            //Set the notification sound
		            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))

		            //Auto cancel will remove the notification once the user touches it
		            .SetAutoCancel(true).Build();

		    //Show the notification
		    notificationManager.Notify(1, notification);
		}
	}
}
