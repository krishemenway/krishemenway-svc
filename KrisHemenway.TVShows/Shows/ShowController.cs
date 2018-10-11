﻿using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Shows
{
	[Route("api/tvshows/shows")]
	public class ShowController : Controller
	{
		[HttpGet(nameof(All))]
		public IActionResult All()
		{
			return Json(new ShowStore().FindAll());
		}
		
		[HttpGet(nameof(Create))]
		public IActionResult Create([FromQuery]CreateShowRequest createShowRequest)
		{
			new RefreshShowTask().Refresh(new ShowStore().Create(createShowRequest));
			return Ok();
		}

		[HttpPost(nameof(RefreshShow))]
		public IActionResult RefreshShow([FromQuery]string name)
		{
			var show = new ShowStore().FindOrNull(name);

			if (show == null)
			{
				return Ok($"Unable to find show with name: {name}");
			}

			new RefreshShowTask().Refresh(show);
			return Ok();
		}
	}
}
