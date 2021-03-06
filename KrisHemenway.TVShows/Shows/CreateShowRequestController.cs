﻿using KrisHemenway.Common;
using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Shows
{
	[Route("shows")]
	public class CreateShowRequestController : ControllerBase
	{
		public CreateShowRequestController(
			IShowStore showStore = null,
			IRefreshShowTask refreshShowTask = null)
		{
			_showStore = showStore ?? new ShowStore();
			_refreshShowTask = refreshShowTask ?? new RefreshShowTask();
		}

		[HttpPost(nameof(Create))]
		[ProducesResponseType(200, Type = typeof(Result))]
		public ActionResult<Result> Create([FromBody] CreateShowRequest createShowRequest)
		{
			var show = _showStore.Create(createShowRequest);
			return _refreshShowTask.Refresh(show);
		}

		private readonly IShowStore _showStore;
		private readonly IRefreshShowTask _refreshShowTask;
	}
}
