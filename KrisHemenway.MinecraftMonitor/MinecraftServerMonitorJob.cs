using System;
using System.Threading.Tasks;
using Humanizer;
using KrisHemenway.Common;
using Microsoft.Extensions.Logging;
using Quartz;

namespace KrisHemenway.MinecraftMonitor
{
	public class MinecraftServerMonitorJob : IJob
	{
		internal static ITrigger CreateTrigger(ServerInfo serverInfo)
		{
			return TriggerBuilder.Create()
				.WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever())
				.WithIdentity(nameof(MinecraftServerMonitorJob), serverInfo.ToString())
				.StartNow()
				.Build();
		}

		internal static IJobDetail CreateJob(ServerInfo serverInfo)
		{
			return JobBuilder
				.Create<MinecraftServerMonitorJob>()
				.WithIdentity(nameof(MinecraftServerMonitorJob), serverInfo.ToString())
				.UsingJobData("Host", serverInfo.Host)
				.UsingJobData("Port", serverInfo.Port)
				.Build();
		}

		public MinecraftServerMonitorJob()
		{
			_pushNotificationSender = new PushNotificationSender();
			_minecraftStatusService = new MinecraftStatusService();
			_minecraftStatusStore = new MinecraftStatusStore();
			_logger = new LoggerFactory().CreateLogger<MinecraftServerMonitorJob>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() => {
				try
				{
					var host = context.JobDetail.JobDataMap.GetString("Host");
					var port = context.JobDetail.JobDataMap.GetInt("Port");
					var serverInfo = ServerInfo.Create(host, port);

					var oldStatus = _minecraftStatusStore.Find(serverInfo);
					var newStatus = _minecraftStatusService.GetStatus(serverInfo);
					_minecraftStatusStore.Save(serverInfo, newStatus);

					if (newStatus.Available && newStatus.PlayersOnline > oldStatus.PlayersOnline)
					{
						_logger.LogDebug($"Minecraft status changed for {serverInfo}");
						SendPushNotification(serverInfo, newStatus);
					}
				}
				catch (Exception e)
				{
					_logger.LogError(default(EventId), e, "Failed to refresh shows!");
				}
			});
		}

		private void SendPushNotification(ServerInfo serverInfo, MinecraftStatus minecraftStatus)
		{
			var details = new PushNotificationDetails
			{
				Title = NotificationTitle,
				Content = $"{"Player".ToQuantity(minecraftStatus.PlayersOnline, ShowQuantityAs.Words).ApplyCase(LetterCasing.Title)} Online\n#{serverInfo.Host}",
				TypeName = nameof(MinecraftServerMonitorJob),
				AlertNumber = minecraftStatus.PlayersOnline
			};

			_pushNotificationSender.NotifyAll(details);
		}

		private const string NotificationTitle = "Minecraft Status";
		private readonly IPushNotificationSender _pushNotificationSender;
		private readonly IMinecraftStatusService _minecraftStatusService;
		private readonly IMinecraftStatusStore _minecraftStatusStore;
		private readonly ILogger<MinecraftServerMonitorJob> _logger;
	}
}