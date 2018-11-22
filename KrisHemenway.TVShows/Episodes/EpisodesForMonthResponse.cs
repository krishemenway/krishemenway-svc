using System.Collections.Generic;

namespace KrisHemenway.TVShows.Episodes
{
	public class EpisodesForMonthResponse
	{
		public bool ShowDownload { get; set; }
		public IReadOnlyList<IEpisode> EpisodesInMonth { get; set; }
	}
}
