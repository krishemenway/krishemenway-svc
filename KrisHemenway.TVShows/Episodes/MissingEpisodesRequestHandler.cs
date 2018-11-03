using KrisHemenway.Common;
using KrisHemenway.TVShows.Shows;
using System.Linq;

namespace KrisHemenway.TVShows.Episodes
{
	public class MissingEpisodesRequestHandler
	{
		public MissingEpisodesRequestHandler(IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		public MissingEpisodesResponse HandleRequest()
		{
			var allShowReports = _showStore.FindAll()
				.Select(CreateReportForShow)
				.ToList();

			return new MissingEpisodesResponse
				{
					AllShows = allShowReports.ToList(),
					TotalMissingEpisodesPercentage = allShowReports
						.Select(show => show.MissingEpisodesPercentage)
						.Aggregate((previousPercentage, missingPercentage) => previousPercentage + missingPercentage),
				};
		}

		private MissingEpisodesForShow CreateReportForShow(IShow show)
		{
			return new MissingEpisodesForShow
				{
					Name = show.Name,
					MissingEpisodes = show.Episodes.Where(episode => episode.IsMissing).ToList(),
					MissingEpisodesPercentage = show.Episodes.CreatePercentageOf(episode => episode.IsMissing),
				};
		}

		private IShowStore _showStore { get; }
	}
}
