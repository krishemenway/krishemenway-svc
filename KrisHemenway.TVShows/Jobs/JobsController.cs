using KrisHemenway.Common;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Jobs
{
	[Route("jobs")]
	public class JobsController : Controller
	{
		[HttpPost(nameof(ExecuteRefreshShowsJob))]
		public IActionResult ExecuteRefreshShowsJob()
		{
			Startup.Scheduler.TriggerJob(RefreshTVShowsJob.JobKey);
			return Json(Result.Successful);
		}
		
		[HttpPost(nameof(ExecuteThisJustInJob))]
		public IActionResult ExecuteThisJustInJob()
		{
			Startup.Scheduler.TriggerJob(ThisJustInJob.JobKey);
			return Json(Result.Successful);
		}
		
		[HttpPost(nameof(ExecuteReleasingTodayJob))]
		public IActionResult ExecuteReleasingTodayJob()
		{
			Startup.Scheduler.TriggerJob(TVReleasingTodayJob.JobKey);
			return Json(Result.Successful);
		}
	}
}