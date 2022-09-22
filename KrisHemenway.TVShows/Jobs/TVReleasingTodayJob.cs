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
	public class TVReleasingTodayJob : IJob
	{
		public TVReleasingTodayJob()
		{
			_pushNotificationSender = new PushNotificationSender();
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() => {
				try
				{
					var episodes = new EpisodeStore().FindEpisodesAiring(DateTime.Today, DateTime.Today);

					if (episodes.Count == 0)
					{
						return;
					}

					var details = new PushNotificationDetails
						{
							Title = $"Airing Today",
							Content = string.Join("\n", episodes.Select(episode => episode.Formatted)),
							TypeName = nameof(TVReleasingTodayJob),
							AlertNumber = episodes.Count
						};

					_pushNotificationSender.NotifyAll(details);
				}
				catch (Exception exception)
				{
					Log.Error(exception, "Failed during TV releasing today job");
				}
			});
		}

		internal static ITrigger CreateTrigger()
		{
			var schedule = DailyTimeIntervalScheduleBuilder.Create()
				.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(20, 00))
				.EndingDailyAfterCount(1)
				.OnEveryDay();

			return TriggerBuilder.Create()
				.WithSchedule(schedule)
				.WithIdentity(nameof(TVReleasingTodayJob))
				.Build();
		}

		internal static JobKey JobKey
		{
			get { return new JobKey(nameof(TVReleasingTodayJob)); }
		}

		internal static IJobDetail CreateJob()
		{
			return JobBuilder.Create<TVReleasingTodayJob>().WithIdentity(JobKey).Build();
		}

		private readonly PushNotificationSender _pushNotificationSender;
	}
}