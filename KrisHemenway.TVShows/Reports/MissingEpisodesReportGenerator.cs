using KrisHemenway.TVShows.Shows;
using System;
using System.Linq;

namespace KrisHemenway.TVShows.Reports
{
	public class MissingEpisodesReportGenerator
	{
		public MissingEpisodesReportGenerator(IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		public MissingEpisodesReport GenerateReport()
		{
			return new MissingEpisodesReport
				{
					AllShows = _showStore.FindAll()
						.Select(CreateReportForShow)
						.Where(show => show.MissingEpisodeCount > 0)
						.ToList(),
				};
		}

		private MissingEpisodesForShowReport CreateReportForShow(IShow show)
		{
			return new MissingEpisodesForShowReport
				{
					Name = show.Name,
					MissingEpisodes = show.Episodes.Where(episode => !episode.HasEpisode && episode.AirDate <= DateTime.Today).ToList(),
					TotalEpisodes = show.Episodes.Count,
				};
		}

		private IShowStore _showStore { get; }
	}
}
