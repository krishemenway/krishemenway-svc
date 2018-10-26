using KrisHemenway.TVShows.Episodes;
using System.Collections.Generic;

namespace KrisHemenway.TVShows.Reports
{
	public class MissingEpisodesReport
	{
		public IReadOnlyList<MissingEpisodesForShowReport> AllShows {get; set; }
	}

	public class MissingEpisodesForShowReport
	{
		public string Name { get; set; }

		public int MissingEpisodeCount => MissingEpisodes.Count;
		public IReadOnlyList<IEpisode> MissingEpisodes { get; set; }

		public int TotalEpisodes { get; set; }
	}
}
