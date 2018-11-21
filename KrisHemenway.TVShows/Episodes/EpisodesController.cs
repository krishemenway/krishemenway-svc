using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class EpisodesController : Controller
	{
		[HttpGet(nameof(RecentlyAdded))]
		[ProducesResponseType(200, Type = typeof(RecentlyAddedEpisodesResponse))]
		public IActionResult RecentlyAdded()
		{
			return Json(new RecentlyAddedEpisodesRequestHandler().HandleRequest());
		}
		
		[HttpGet(nameof(Upcoming))]
		[ProducesResponseType(200, Type = typeof(UpcomingEpisodesResponse))]
		public IActionResult Upcoming()
		{
			return Json(new UpcomingEpisodesRequestHandler().HandleRequest());
		}

		[HttpGet(nameof(Missing))]
		[ProducesResponseType(200, Type = typeof(MissingEpisodesResponse))]
		public IActionResult Missing()
		{
			return Json(new MissingEpisodesRequestHandler().HandleRequest());
		}
		
		[HttpGet("calendar/{year}/{month}")]
		[ProducesResponseType(200, Type = typeof(EpisodesForMonthResponse))]
		public IActionResult GetEpisodesForMonth([FromRoute]EpisodesForMonthRequest request)
		{
			return Json(new EpisodesForMonthRequestHandler().HandleRequest(request));
		}

		[HttpGet("download")]
		[DownloadAuthenticationRequired]
		public IActionResult DownloadEpisode(DownloadEpisodeRequest request)
		{
			var response = new DownloadEpisodeRequestHandler().HandleRequest(request);

			if (!response.Success)
			{
				return Ok("Something went wrong with the download");
			}

			return File(response.Data.FileStream, response.Data.ContentType, response.Data.FileName);
		}

		[HttpGet("authenticate")]
		public IActionResult AuthenticateRequest([FromQuery]DownloadAuthenticationRequest request)
		{
			return Json(new DownloadAuthenticationRequestHandler().HandleRequest(request, HttpContext.Session));
		}
	}
}
