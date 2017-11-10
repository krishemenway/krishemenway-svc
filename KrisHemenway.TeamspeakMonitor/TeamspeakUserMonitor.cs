using Serilog;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace KrisHemenway.TeamspeakMonitor
{
	public class TeamspeakUserMonitor
	{
		static TeamspeakUserMonitor()
		{
			var hub = new NotificationHub();
			hub.ClientJoined.Triggered += ClientJoined_Triggered;

			QueryClient = new QueryClient(Program.Settings.TeamspeakQueryHost, notificationHub: hub);
		}

		public TeamspeakUserMonitor(IQueryClient queryClient = null)
		{
			_client = queryClient ?? QueryClient;
		}

		public void StartMonitoring()
		{
			var connectResponse = QueryClient.Connect();

			if (connectResponse.Success)
			{
				var loggedIn = !new LoginCommand(Program.Settings.TeamspeakQueryUser, Program.Settings.TeamspeakQueryPassword).Execute(QueryClient).IsErroneous;
				if (loggedIn)
				{
					Log.Information("Successfully connected with supplied credentials");
				}
				else
				{
					Log.Error("Failed to login with supplied credentials");
					return;
				}

				var switchedPort = !new UseCommand(Program.Settings.TeamspeakQueryPort).Execute(QueryClient).IsErroneous;
				if (switchedPort)
				{
					Log.Information($"Successfully switched port to {Program.Settings.TeamspeakQueryPort}");
				}
				else
				{
					Log.Error($"Failed to switch to port {Program.Settings.TeamspeakQueryPort}");
					return;
				}
			}
			else
			{
				Log.Error($"Could not connect to server {Program.Settings.TeamspeakQueryHost}:{Program.Settings.TeamspeakQueryPort}");
			}
		}

		private static void ClientJoined_Triggered(object sender, ClientJoinedEventArgs e)
		{
			new TeamspeakNotificationSender().SendPushNotification(new TeamspeakUser { NickName = e.Nickname });
		}

		private IQueryClient _client { get; set; }

		private static QueryClient QueryClient { get; set; }
	}
}
