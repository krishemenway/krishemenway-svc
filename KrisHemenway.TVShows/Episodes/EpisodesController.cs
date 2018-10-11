using KrisHemenway.TVShows.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class EpisodesController : Controller
	{
		[HttpGet("new")]
		public IActionResult GetNewEpisodes()
		{
			var episodes = new EpisodeStore().FindNewEpisodes();
			return Json(episodes);
		}
		
		[HttpGet("upcoming")]
		public IActionResult GetUpcomingEpisodes()
		{
			var episodes = new EpisodeStore().FindEpisodesAiring(DateTime.Today, DateTime.Today.AddDays(3));
			return Json(episodes);
		}

		[HttpGet("missing")]
		public IActionResult MissingEpisodes()
		{
			return Json(new MissingEpisodesReportGenerator().GenerateReport());
		}
		
		[HttpGet("calendar/{year}/{month}")]
		public IActionResult GetEpisodesForMonth(int year, int month)
		{
			var beginningOfMonth = new DateTime(year, month, 1);
			var endOfMonth = beginningOfMonth.AddMonths(1).AddDays(-1);

			var episodesInMonth = new EpisodeStore()
				.FindEpisodesAiring(beginningOfMonth, endOfMonth)
				.OrderBy(x => x.AirDate)
				.ThenBy(x => x.ShowId)
				.ThenBy(x => x.Season)
				.ThenBy(x => x.EpisodeInSeason);

			return Json(new { EpisodesInMonth = episodesInMonth });
		}
	}
}
