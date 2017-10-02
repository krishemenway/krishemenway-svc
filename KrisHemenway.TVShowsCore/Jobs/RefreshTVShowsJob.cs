using KrisHemenway.TVShowsCore.Seriess;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KrisHemenway.TVShowsCore.Jobs
{
	[DisallowConcurrentExecution]
	public class RefreshTVShowsJob : IJob
	{
		public RefreshTVShowsJob()
		{
			_seriesStore = new SeriesStore();
			_refreshSeriesTask = new RefreshSeriesTask();

			_logger = new LoggerFactory().CreateLogger<RefreshTVShowsJob>();
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() => {
				try
				{
					foreach (var series in _seriesStore.FindAll())
					{
						_refreshSeriesTask.Refresh(series);
						Thread.Sleep(1000);
					}
				}
				catch (Exception e)
				{
					_logger.LogError(default(EventId), e, "Failed to refresh shows!");
				}
			});
		}

		internal static IJobDetail CreateJob()
		{
			return JobBuilder.Create<RefreshTVShowsJob>().WithIdentity(JobKey).Build();
		}

		internal static JobKey JobKey
		{
			get { return new JobKey(nameof(RefreshTVShowsJob)); }
		}

		internal static ITrigger CreateTrigger()
		{
			var schedule = DailyTimeIntervalScheduleBuilder
				.Create()
				.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(JobExecutionHour, JobExecutionMinute))
				.EndingDailyAfterCount(1)
				.OnEveryDay();

			return TriggerBuilder.Create()
				.WithSchedule(schedule)
				.WithIdentity(nameof(RefreshTVShowsJob))
				.StartNow()
				.Build();
		}

		internal const int JobExecutionHour = 8;
		internal const int JobExecutionMinute = 0;

		private readonly ISeriesStore _seriesStore;
		private readonly IRefreshSeriesTask _refreshSeriesTask;

		private readonly ILogger<RefreshTVShowsJob> _logger;
	}
}