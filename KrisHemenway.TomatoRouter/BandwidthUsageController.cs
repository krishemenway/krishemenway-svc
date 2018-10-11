using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TomatoRouter
{
	[Route("api/router/bandwidth")]
	public class BandwidthUsageController : Controller
	{
		[HttpGet("latest_month")]
		public IActionResult LatestMonth()
		{
			return Json(new { });
		}

		[HttpGet("refresh")]
		public IActionResult Refresh()
		{
			Startup.Scheduler.TriggerJob(RefreshDailyBandwidthJob.JobKey);
			return Json(new { });
		}
	}
}
