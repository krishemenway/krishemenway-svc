using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Seriess
{
	[Route("api/tvshows/series")]
	public class ShowController : Controller
	{
		[HttpGet(nameof(All))]
		public IActionResult All()
		{
			return Json(new ShowStore().FindAll());
		}
		
		[HttpGet(nameof(Create))]
		public IActionResult Create([FromQuery]CreateSeriesRequest createSeriesRequest)
		{
			new RefreshSeriesTask().Refresh(new ShowStore().Create(createSeriesRequest));
			return Ok();
		}

		[HttpPost(nameof(RefreshSeries))]
		public IActionResult RefreshSeries([FromQuery]string name)
		{
			var series = new ShowStore().FindOrNull(name);

			if (series == null)
			{
				return Ok($"Unable to find series with name: {name}");
			}

			new RefreshSeriesTask().Refresh(series);
			return Ok();
		}
	}
}
