using System.Linq;

namespace KrisHemenway.TomatoRouter
{
	public class RefreshDailyBandwidthTask
	{
		public RefreshDailyBandwidthTask(
			ITomatoRouterHttpClient tomatoRouterHttpClient = null,
			IDailyBandwidthStore dailyBandwidthStore = null)
		{
			_tomatoRouterHttpClient = tomatoRouterHttpClient ?? new TomatoRouterHttpClient();
			_dailyBandwidthStore = dailyBandwidthStore ?? new DailyBandwidthStore();
		}

		public void Execute()
		{
			var latestUsages = _tomatoRouterHttpClient.LatestDailyBandwidthUsage().OrderBy(x => x.Date).ToList();

			var allExistingBandwidths = _dailyBandwidthStore
				.Find(latestUsages.First().Date, latestUsages.Last().Date)
				.ToDictionary(x => x.Date, x => x);

			foreach (var latestUsage in latestUsages)
			{
				if (allExistingBandwidths.ContainsKey(latestUsage.Date))
				{
					if (!allExistingBandwidths[latestUsage.Date].Equals(latestUsage))
					{
						_dailyBandwidthStore.Update(latestUsage);
					}
				}
				else
				{
					_dailyBandwidthStore.Create(latestUsage);
				}
			}
		}

		private readonly ITomatoRouterHttpClient _tomatoRouterHttpClient;
		private readonly IDailyBandwidthStore _dailyBandwidthStore;
	}
}
