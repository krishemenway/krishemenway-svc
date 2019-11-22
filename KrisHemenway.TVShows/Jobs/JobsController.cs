using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Jobs
{
	[Route("jobs")]
	public class JobsController : ControllerBase
	{
		[HttpPost(nameof(ExecuteRefreshShowsJob))]
		public ActionResult ExecuteRefreshShowsJob()
		{
			Startup.Scheduler.TriggerJob(RefreshTVShowsJob.JobKey);
			return Ok();
		}
		
		[HttpPost(nameof(ExecuteThisJustInJob))]
		public ActionResult ExecuteThisJustInJob()
		{
			Startup.Scheduler.TriggerJob(ThisJustInJob.JobKey);
			return Ok();
		}
		
		[HttpPost(nameof(ExecuteReleasingTodayJob))]
		public ActionResult ExecuteReleasingTodayJob()
		{
			Startup.Scheduler.TriggerJob(TVReleasingTodayJob.JobKey);
			return Ok();
		}
	}
}