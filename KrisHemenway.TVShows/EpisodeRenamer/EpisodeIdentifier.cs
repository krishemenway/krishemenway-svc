using KrisHemenway.Common;
using KrisHemenway.TVShows.Episodes;
using System.Linq;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IEpisodeIdentifier
	{
		Result<IEpisode> Identify(IVideoFile file);
	}

	public class EpisodeIdentifier : IEpisodeIdentifier
	{
		public EpisodeIdentifier(IEpisodeFilenameParser episodeFilenameParser = null)
		{
			_episodeFilenameParser = episodeFilenameParser ?? new EpisodeFilenameParser();
		}

		public Result<IEpisode> Identify(IVideoFile file)
		{
			var parsedEpisodeResult = _episodeFilenameParser.TryParseVideoFile(file);
			if (!parsedEpisodeResult.Success)
			{
				return Result<IEpisode>.Failure(parsedEpisodeResult.ErrorMessage);
			}

			var parsedEpisode = parsedEpisodeResult.Data;
			var episode = parsedEpisodeResult.Data.Show.Episodes.SingleOrDefault(e => e.Season == parsedEpisode.Season && e.EpisodeInSeason == parsedEpisode.EpisodeNumberInSeason);

			if (episode == null)
			{
				return Result<IEpisode>.Failure($"Could not find episode with collected information: {parsedEpisode.Show.Name} S{parsedEpisode.Season}E{parsedEpisode.EpisodeNumberInSeason}");
			}

			return Result<IEpisode>.Successful(episode);
		}

		private readonly IEpisodeFilenameParser _episodeFilenameParser;
		private readonly IEpisodeStore _episodeStore;
	}
}