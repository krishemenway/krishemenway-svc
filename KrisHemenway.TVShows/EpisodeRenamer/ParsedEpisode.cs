using KrisHemenway.TVShows.Shows;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public class ParsedEpisode
    {
		public IShow Show { get; set; }
		public int Season { get; set; }
		public int EpisodeNumberInSeason { get; set; }
		public IVideoFile VideoFile { get; set; }
	}
}
