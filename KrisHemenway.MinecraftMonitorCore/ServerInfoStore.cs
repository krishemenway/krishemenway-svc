using KrisHemenway.CommonCore;
using System.Collections.Generic;

namespace KrisHemenway.MinecraftMonitorCore
{
	public interface IServerInfoStore
	{
		IReadOnlyList<ServerInfo> Find();
	}

	public class ServerInfoStore : IServerInfoStore
	{
		public IReadOnlyList<ServerInfo> Find()
		{
			return new List<ServerInfo>
			{
				ServerInfo.Create("minecraft.colinlorenz.com", 25565),
				ServerInfo.Create("minecraft.krishemenway.com", 25565)
			};
		}
	}
}
