using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Gms.Gcm.Iid;
using Android.Gms.Gcm;
using System.Net;

namespace KrisHemenway.AndroidApp
{
	[Service(Exported = false)]
	public class NotificationIntentService : IntentService
	{
		protected override void OnHandleIntent(Intent intent)
		{
			try
			{
				Android.Util.Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");

				lock (tokenLock)
				{
					var instanceID = InstanceID.GetInstance(this);
					var token = instanceID.GetToken("YOUR_SENDER_ID", GoogleCloudMessaging.InstanceIdScope);

					Android.Util.Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
					SendRegistrationToAppServer(token);
					Subscribe(token);
				}
			}
			catch (Exception e)
			{
				Android.Util.Log.Debug("RegistrationIntentService", "Failed to get a registration token");
				return;
			}
		}

		private void SendRegistrationToAppServer(string token)
		{
			// Add custom implementation here as needed
		}

		private void Subscribe(string token)
		{
			var pubSub = GcmPubSub.GetInstance(this);
			pubSub.Subscribe(token, "/topics/global", null);
		}
		
		static object tokenLock = new object();
	}
}