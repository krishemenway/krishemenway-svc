using KrisHemenway.CommonCore;
using KrisHemenway.TVShowsCore.Episodes;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KrisHemenway.TVShowsCore.Jobs
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
				var episodes = new EpisodeStore().FindEpisodesAiring(DateTime.Today, DateTime.Today);

				if (episodes.Count == 0)
				{
					return;
				}

				var details = new PushNotificationDetails
				{
					Title = $"Airing Today",
					Content = string.Join("\n", episodes.Select(episode => $"{episode.Series} - {episode.Season}x{episode.EpisodeInSeason} {episode.Title}")),
					TypeName = nameof(TVReleasingTodayJob),
					AlertNumber = episodes.Count
				};

				_pushNotificationSender.NotifyAll(details);
			});
		}

		internal static ITrigger CreateTrigger()
		{
			var schedule = DailyTimeIntervalScheduleBuilder.Create()
				.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 45))
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