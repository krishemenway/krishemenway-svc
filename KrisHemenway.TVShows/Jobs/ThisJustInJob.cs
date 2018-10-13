using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using Quartz;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KrisHemenway.TVShows.Jobs
{
	[DisallowConcurrentExecution]
	public class ThisJustInJob : IJob
	{
		public ThisJustInJob()
		{
			_pushNotificationSender = new PushNotificationSender();
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() =>
			{
				try
				{
					var newEpisodes = new EpisodeStore().FindNewEpisodes();

					if (newEpisodes.Count == 0)
					{
						return;
					}

					var details = new PushNotificationDetails
						{
							Title = NewInformationTodayTitle,
							Content = string.Join("\n", newEpisodes.Select(episode => $"{episode.ShowName} - {episode.Season}x{episode.EpisodeInSeason} - {episode.Title}")),
							TypeName = nameof(ThisJustInJob),
							AlertNumber = newEpisodes.Count
						};

					_pushNotificationSender.NotifyAll(details);
				}
				catch (Exception exception)
				{
					Log.Error(exception, "Failed during This Just in Job");
				}
			});
		}

		internal static ITrigger CreateTrigger()
		{
			var schedule = DailyTimeIntervalScheduleBuilder.Create()
				.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 30))
				.EndingDailyAfterCount(1)
				.OnEveryDay();

			return TriggerBuilder.Create()
				.WithSchedule(schedule)
				.WithIdentity(nameof(ThisJustInJob))
				.Build();
		}

		internal static JobKey JobKey
		{
			get { return new JobKey(nameof(ThisJustInJob)); }
		}

		internal static IJobDetail CreateJob()
		{
			return JobBuilder.Create<ThisJustInJob>().WithIdentity(JobKey).Build();
		}

		private const string NewInformationTodayTitle = "This Just In!";

		private readonly PushNotificationSender _pushNotificationSender;
	}
}