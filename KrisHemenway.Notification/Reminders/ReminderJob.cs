using KrisHemenway.Common;
using KrisHemenway.Notification.SentNotifications;
using Quartz;
using Serilog;
using System;
using System.Threading.Tasks;

namespace KrisHemenway.Notification.Reminders
{
	[DisallowConcurrentExecution]
	public class ReminderJob : IJob
	{
		public ReminderJob() { }

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() => {
				try
				{
					var details = new PushNotificationDetails
						{
							Title = "Hey You!",
							Content = "Reminder to create a video game! Start today!",
						};

					new SendNotificationRequestHandler().HandleRequest(details);
				}
				catch (Exception e)
				{
					Log.Error(e, "Failed to send reminder!");
				}
			});
		}

		internal static IJobDetail CreateJob()
		{
			return JobBuilder.Create<ReminderJob>().WithIdentity(JobKey).Build();
		}

		internal static JobKey JobKey
		{
			get { return new JobKey(nameof(ReminderJob)); }
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
				.WithIdentity(nameof(ReminderJob))
				.StartNow()
				.Build();
		}

		internal const int JobExecutionHour = 18;
		internal const int JobExecutionMinute = 0;
	}
}
