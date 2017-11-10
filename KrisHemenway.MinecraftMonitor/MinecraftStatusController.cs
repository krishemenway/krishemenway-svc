using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace KrisHemenway.MinecraftMonitor
{
	[Route("api/minecraft")]
	public class MinecraftStatusController : Controller
	{
		[HttpGet(nameof(Status))]
		public IActionResult Status(IOptions<Settings> settings)
		{
			var minecraftStatusStore = new MinecraftStatusStore();
			var minecraftStatus = new ServerInfoStore(settings)
				.Find()
				.Select(x => new {
					x.Host,
					x.Port,
					LastChecked = minecraftStatusStore.Find(x).StatusTime,
					minecraftStatusStore.Find(x).Available,
					minecraftStatusStore.Find(x).PlayersOnline
				});

			return Ok(minecraftStatus);
		}
	}
}
