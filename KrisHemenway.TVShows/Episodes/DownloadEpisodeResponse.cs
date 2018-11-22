using System.IO;

namespace KrisHemenway.TVShows.Episodes
{
	public class DownloadEpisodeResponse
	{
		public FileStream FileStream { get; set; }
		public string ContentType { get; set; }
		public string FileName { get; set; }
	}
}
