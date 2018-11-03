using KrisHemenway.Common;
using KrisHemenway.TVShows.Jobs;

namespace KrisHemenway.TVShows.Shows
{
	public class CreateShowRequestHandler
	{
		public CreateShowRequestHandler(
			IShowStore showStore = null,
			IRefreshShowTask refreshShowTask = null)
		{
			_showStore = showStore ?? new ShowStore();
			_refreshShowTask = refreshShowTask ?? new RefreshShowTask();
		}

		public Result HandleRequest(CreateShowRequest createShowRequest)
		{
			var show = _showStore.Create(createShowRequest);
			_refreshShowTask.Refresh(show);

			return Result.Successful;
		}

		private readonly IShowStore _showStore;
		private readonly IRefreshShowTask _refreshShowTask;
	}
}
