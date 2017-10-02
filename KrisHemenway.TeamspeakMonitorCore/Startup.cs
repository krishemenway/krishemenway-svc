using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace KrisHemenway.TeamspeakMonitorCore
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			NotificationsHub = new NotificationHub();
			NotificationsHub.ClientJoined.Triggered += ClientJoined_Triggered;

			QueryClient = new QueryClient(Program.Configuration.GetValue<string>("TeamspeakQueryHost"), notificationHub: NotificationsHub);
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonFormatters(FixJsonCamelCasing);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
		{
			loggerFactory.AddConsole(Program.Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseMvc();

			ConnectToTeamspeak();
		}

		private void ConnectToTeamspeak()
		{
			var connectResponse = QueryClient.Connect();

			var logger = new LoggerFactory().CreateLogger<Startup>();
			if (connectResponse.Success)
			{
				var user = Program.Configuration.GetValue<string>("TeamspeakQueryUser");
				var pass = Program.Configuration.GetValue<string>("TeamspeakQueryPassword");
				var loggedIn = !new LoginCommand(user, pass).Execute(QueryClient).IsErroneous;
				if (loggedIn)
				{
					logger.LogInformation("Successfully connected with supplied credentials");
				}
				else
				{
					logger.LogError("Failed to login with supplied credentials");
					return;
				}

				var port = Program.Configuration.GetValue<ushort>("TeamspeakQueryPort");
				var switchedPort = !new UseCommand(port).Execute(QueryClient).IsErroneous;
				if (loggedIn)
				{
					logger.LogInformation($"Successfully switched port to {port}");
				}
				else
				{
					logger.LogError($"Failed to switch to port {port}");
					return;
				}
			}
			else
			{
				new LoggerFactory().CreateLogger<Startup>().LogError($"Could not connect to server {Program.Configuration.GetValue<string>("TeamspeakQueryHost")}:{Program.Configuration.GetValue<ushort?>("TeamspeakQueryPort")}");
			}
		}

		private static void ClientJoined_Triggered(object sender, ClientJoinedEventArgs e)
		{
			var user = new TeamspeakUser { NickName = e.Nickname };
			new TeamspeakNotificationSender().SendPushNotification(user);
		}

		private void FixJsonCamelCasing(JsonSerializerSettings settings)
		{
			var resolver = settings.ContractResolver;
			if (resolver != null)
			{
				var res = resolver as DefaultContractResolver;
				res.NamingStrategy = null;  // <<!-- this removes the camelcasing
			}
		}

		public static QueryClient QueryClient { get; set; }
		public static NotificationHub NotificationsHub { get; set; }
	}
}
