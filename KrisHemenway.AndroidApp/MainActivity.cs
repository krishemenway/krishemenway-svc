using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Gms.Common;
using Android.Content;

namespace KrisHemenway.AndroidApp
{
	[Activity(Label = "Notifications", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			NotificationsList = new Lazy<ListView>(() => FindViewById<ListView>(Resource.Id.NotificationsList));
			NotificationsList.Value.Adapter = new NotificationsAdapter();

			if (IsPlayServicesAvailable())
			{
				var intent = new Intent(this, typeof(NotificationIntentService));
				StartService(intent);
			}
		}

		private bool IsPlayServicesAvailable()
		{
			return GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this) == ConnectionResult.Success;
		}

		private Lazy<ListView> NotificationsList { get; set; }
	}
}

