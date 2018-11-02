using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using System.Collections.Generic;

namespace KrisHemenway.TVShows.Reports
{
	public class MissingEpisodesReport
	{
		public IReadOnlyList<MissingEpisodesForShowReport> AllShows {get; set; }
		public Percentage TotalMissingEpisodesPercentage { get; set; }
	}

	public class MissingEpisodesForShowReport
	{
		public string Name { get; set; }
		public IReadOnlyList<IEpisode> MissingEpisodes { get; set; }
		public Percentage MissingEpisodesPercentage { get; set; }
	}
}
