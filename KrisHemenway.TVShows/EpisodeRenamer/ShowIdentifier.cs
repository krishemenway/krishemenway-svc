using KrisHemenway.TVShows.Shows;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IShowIdentifier
	{
		bool TryIdentifyVideo(IVideoFile file, out IShow show);
	}

	public class ShowIdentifier : IShowIdentifier
	{
		public ShowIdentifier(IShowStore showStore = null)
		{
			_showStore = showStore ?? new ShowStore();
		}

		public bool TryIdentifyVideo(IVideoFile file, out IShow show)
		{
			show = _showStore.FindByPath(file.GetAllParentDirectoryPaths());
			return show != null;
		}

		private readonly IShowStore _showStore;
	}
}
