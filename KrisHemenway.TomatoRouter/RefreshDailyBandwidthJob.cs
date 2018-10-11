using Quartz;
using Serilog;
using System;
using System.Threading.Tasks;

namespace KrisHemenway.TomatoRouter
{
	[DisallowConcurrentExecution]
    public class RefreshDailyBandwidthJob : IJob
	{
		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() => {
				try
				{
					new RefreshDailyBandwidthTask().Execute();
				}
				catch (Exception e)
				{
					Log.Error(e, "Failed to refresh bandwidth data!");
				}
			});
		}

		internal static IJobDetail CreateJob()
		{
			return JobBuilder.Create<RefreshDailyBandwidthJob>().WithIdentity(JobKey).Build();
		}

		internal static JobKey JobKey
		{
			get { return new JobKey(nameof(RefreshDailyBandwidthJob)); }
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
				.WithIdentity(nameof(RefreshDailyBandwidthJob))
				.StartNow()
				.Build();
		}

		internal const int JobExecutionHour = 2;
		internal const int JobExecutionMinute = 0;
	}
}
