using System.Collections.Generic;

namespace KrisHemenway.TVShows.Shows
{
	public class AllShowsRequestHandler
	{
		public AllShowsRequestHandler(IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		public AllShowsResponse HandleRequest()
		{
			return new AllShowsResponse
				{
					Shows = _showStore.FindAll(),
				};
		}

		private readonly IShowStore _showStore;
	}

	public class AllShowsResponse
	{
		public IReadOnlyList<IShow> Shows { get; set; }
	}
}
