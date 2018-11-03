namespace KrisHemenway.TVShows.Episodes
{
	public class RecentlyAddedEpisodesRequestHandler
	{
		public RecentlyAddedEpisodesRequestHandler(IEpisodeStore episodeStore = null)
		{
			_episodeStore = episodeStore ?? new EpisodeStore();
		}

		public RecentlyAddedEpisodesResponse HandleRequest()
		{
			return new RecentlyAddedEpisodesResponse
				{
					Episodes = _episodeStore.FindNewEpisodes(),
				};
		}

		private readonly IEpisodeStore _episodeStore;
	}
}
