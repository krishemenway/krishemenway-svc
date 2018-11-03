using KrisHemenway.Common;
using System.Collections.Generic;

namespace KrisHemenway.TVShows.Episodes
{
	public class MissingEpisodesResponse
	{
		public IReadOnlyList<MissingEpisodesForShow> AllShows {get; set; }
		public Percentage TotalMissingEpisodesPercentage { get; set; }
	}

	public class MissingEpisodesForShow
	{
		public string Name { get; set; }
		public IReadOnlyList<IEpisode> MissingEpisodes { get; set; }
		public Percentage MissingEpisodesPercentage { get; set; }
	}
}
