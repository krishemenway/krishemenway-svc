using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;

namespace KrisHemenway.TVShows
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonOptions(FixJsonCamelCasing);
			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromDays(1);
				options.Cookie.HttpOnly = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostApplicationLifetime hostApplicationLifetime)
		{
			app.UseSession();
			app.UseRouting();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;
			Scheduler.ScheduleJob(RefreshTVShowsJob.CreateJob(), RefreshTVShowsJob.CreateTrigger());
			Scheduler.ScheduleJob(TVReleasingTodayJob.CreateJob(), TVReleasingTodayJob.CreateTrigger());
			Scheduler.ScheduleJob(ThisJustInJob.CreateJob(), ThisJustInJob.CreateTrigger());
			Scheduler.Start(hostApplicationLifetime.ApplicationStopping);
		}

		private void FixJsonCamelCasing(JsonOptions options)
		{
			// this unsets the default behavior (camelCase); "what you see is what you get" is now default
			options.JsonSerializerOptions.PropertyNamingPolicy = null;
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
