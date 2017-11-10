using KrisHemenway.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace KrisHemenway.MinecraftMonitor
{
	public interface IServerInfoStore
	{
		IReadOnlyList<ServerInfo> Find();
	}

	public class ServerInfoStore : IServerInfoStore
	{
		public IReadOnlyList<ServerInfo> Find()
		{
			return Program.Configuration
				.GetValue<string[]>("Servers")?
				.Select(CreateServerInfo)
				.ToList() ?? new List<ServerInfo>();
		}

		private ServerInfo CreateServerInfo(string serverAddress)
		{
			var serverAddressParts = serverAddress.Split(new[] { ':' });

			return new ServerInfo
				{
					Host = serverAddressParts[0],
					Port = Convert.ToInt32(serverAddressParts[1])
				};
		}
	}
}
