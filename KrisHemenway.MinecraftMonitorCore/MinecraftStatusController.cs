using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KrisHemenway.MinecraftMonitorCore
{
	[Route("api/minecraft")]
	public class MinecraftStatusController : Controller
	{
		[HttpGet(nameof(Status))]
		public IActionResult Status()
		{
			var minecraftStatusStore = new MinecraftStatusStore();
			var minecraftStatus = new ServerInfoStore()
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
