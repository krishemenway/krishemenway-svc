using System.Collections.Generic;

namespace KrisHemenway.TVShows.Episodes
{
	public class EpisodesForMonthResponse
	{
		public IReadOnlyList<IEpisode> EpisodesInMonth { get; set; }
	}
}
