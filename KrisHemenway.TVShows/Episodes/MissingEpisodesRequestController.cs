using KrisHemenway.TVShows.Shows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Linq;
using System.Threading;

namespace KrisHemenway.TVShows.Episodes
{
	[Route("api/tvshows/episodes")]
	public class MissingEpisodesRequestController : ControllerBase
	{
		public MissingEpisodesRequestController(
			IMemoryCache memoryCache,
			IShowStore showStore = null)
		{
			_memoryCache = memoryCache;
			_showStore = showStore ?? new ShowStore();
		}

		[HttpGet(nameof(Missing))]
		[ProducesResponseType(200, Type = typeof(MissingEpisodesResponse))]
		public ActionResult<MissingEpisodesResponse> Missing()
		{
			return _memoryCache.GetOrCreate("MissingShowsReport", (cache) => {

				CacheSource = new CancellationTokenSource();
				cache.AddExpirationToken(new CancellationChangeToken(CacheSource.Token));
				cache.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

				return CreatingMissingEpisodesReport();
			});
		}

		private MissingEpisodesResponse CreatingMissingEpisodesReport()
		{
			Log.Information("Building missing episodes report");
			var allShowReports = _showStore.FindAll().Select(CreateReportForShow).ToList();

			return new MissingEpisodesResponse
				{
					AllShows = allShowReports.ToList(),
					TotalMissingEpisodesPercentage = allShowReports
						.Select(show => show.MissingEpisodesPercentage)
						.Aggregate((previousPercentage, missingPercentage) => previousPercentage + missingPercentage),
				};
		}

		private MissingEpisodesForShow CreateReportForShow(IShow show)
		{
			return new MissingEpisodesForShow
				{
					Name = show.Name,
					MissingEpisodes = show.Episodes.Where(episode => episode.IsMissing).ToList(),
					MissingEpisodesPercentage = show.Episodes.CreatePercentageOf(episode => episode.IsMissing),
				};
		}

		internal static CancellationTokenSource CacheSource { get; private set; }

		private readonly IMemoryCache _memoryCache;
		private readonly IShowStore _showStore;
	}
}
