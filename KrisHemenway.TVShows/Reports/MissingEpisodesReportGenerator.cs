using KrisHemenway.Common;
using KrisHemenway.TVShows.Shows;
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
			var allShowReports = _showStore.FindAll()
				.Select(CreateReportForShow)
				.ToList();

			return new MissingEpisodesReport
				{
					AllShows = allShowReports.ToList(),
					TotalMissingEpisodesPercentage = allShowReports
						.Select(show => show.MissingEpisodesPercentage)
						.Aggregate((previousPercentage, missingPercentage) => previousPercentage + missingPercentage),
				};
		}

		private MissingEpisodesForShowReport CreateReportForShow(IShow show)
		{
			return new MissingEpisodesForShowReport
				{
					Name = show.Name,
					MissingEpisodes = show.Episodes.Where(episode => episode.IsMissing).ToList(),
					MissingEpisodesPercentage = show.Episodes.CreatePercentageOf(episode => episode.IsMissing),
				};
		}

		private IShowStore _showStore { get; }
	}
}
