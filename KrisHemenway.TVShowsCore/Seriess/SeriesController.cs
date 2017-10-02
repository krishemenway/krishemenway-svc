using KrisHemenway.TVShowsCore.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShowsCore.Seriess
{
	[Route("api/tvshows/series")]
	public class SeriesController : Controller
	{
		[HttpGet(nameof(All))]
		public IActionResult All()
		{
			return Json(new SeriesStore().FindAll());
		}
		
		[HttpGet(nameof(Create))]
		public IActionResult Create([FromQuery]CreateSeriesRequest createSeriesRequest)
		{
			new RefreshSeriesTask().Refresh(new SeriesStore().Create(createSeriesRequest));
			return Ok();
		}

		[HttpPost(nameof(RefreshSeries))]
		public IActionResult RefreshSeries([FromQuery]string name)
		{
			var series = new SeriesStore().FindOrNull(name);

			if (series == null)
			{
				return Ok($"Unable to find series with name: {name}");
			}

			new RefreshSeriesTask().Refresh(series);
			return Ok();
		}
	}
}
