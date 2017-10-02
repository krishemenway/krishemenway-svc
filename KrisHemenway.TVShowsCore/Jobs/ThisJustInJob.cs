using KrisHemenway.CommonCore;
using KrisHemenway.TVShowsCore.Episodes;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace KrisHemenway.TVShowsCore.Jobs
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
				var newEpisodes = new EpisodeStore().FindNewEpisodes();

				if (newEpisodes.Count == 0)
				{
					return;
				}

				var details = new PushNotificationDetails
				{
					Title = NewInformationTodayTitle,
					Content = string.Join("\n", newEpisodes.Select(episode => $"{episode.Series} - {episode.Season}x{episode.EpisodeInSeason} {episode.Title}")),
					TypeName = nameof(ThisJustInJob),
					AlertNumber = newEpisodes.Count
				};

				_pushNotificationSender.NotifyAll(details);
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