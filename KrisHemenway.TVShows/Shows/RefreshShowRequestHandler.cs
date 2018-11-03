using KrisHemenway.Common;
using KrisHemenway.TVShows.Jobs;

namespace KrisHemenway.TVShows.Shows
{
	public class RefreshShowRequestHandler
	{
		public RefreshShowRequestHandler(
			IShowStore showStore = null,
			IRefreshShowTask refreshShowTask = null)
		{
			_showStore = showStore ?? new ShowStore();
			_refreshShowTask = refreshShowTask ?? new RefreshShowTask();
		}

		public Result HandleRequest(RefreshShowRequest request)
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
