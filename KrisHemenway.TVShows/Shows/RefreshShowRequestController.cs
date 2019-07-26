using KrisHemenway.Common;
using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.TVShows.Shows
{
	[Route("api/tvshows/shows")]
	public class RefreshShowRequestController : ControllerBase
	{
		public RefreshShowRequestController(
			IShowStore showStore = null,
			IRefreshShowTask refreshShowTask = null)
		{
			_showStore = showStore ?? new ShowStore();
			_refreshShowTask = refreshShowTask ?? new RefreshShowTask();
		}

		[HttpPost(nameof(RefreshShow))]
		[ProducesResponseType(200, Type = typeof(Result))]
		public Result RefreshShow([FromBody] RefreshShowRequest request)
		{
			if (!_showStore.TryFindByName(request.Name, out var show))
			{
				return Result.Failure($"Unable to find show: {request.Name}");
			}

			return _refreshShowTask.Refresh(show);
		}

		private readonly IShowStore _showStore;
		private readonly IRefreshShowTask _refreshShowTask;
	}
}
