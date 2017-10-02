using KrisHemenway.CommonCore;
using System.Collections.Generic;

namespace KrisHemenway.MinecraftMonitorCore
{
	public interface IMinecraftStatusStore
	{
		MinecraftStatus Find(ServerInfo serverInfo);
		void Save(ServerInfo serverInfo, MinecraftStatus status);
	}

	public class MinecraftStatusStore : IMinecraftStatusStore
	{
		static MinecraftStatusStore()
		{
			LatestStatus = new Dictionary<ServerInfo, MinecraftStatus>();
		}

		public void Save(ServerInfo serverInfo, MinecraftStatus status)
		{
			if (!LatestStatus.ContainsKey(serverInfo))
			{
				LatestStatus.Add(serverInfo, status);
			}

			LatestStatus[serverInfo] = status;
		}

		public MinecraftStatus Find(ServerInfo serverInfo)
		{
			if(!LatestStatus.ContainsKey(serverInfo))
			{
				return MinecraftStatus.Default;
			}

			return LatestStatus[serverInfo];
		}

		private static readonly IDictionary<ServerInfo, MinecraftStatus> LatestStatus;
	}
}
