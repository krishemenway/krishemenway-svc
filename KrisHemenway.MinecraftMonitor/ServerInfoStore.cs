using KrisHemenway.Common;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Microsoft.Extensions.Options;

namespace KrisHemenway.MinecraftMonitor
{
	public interface IServerInfoStore
	{
		IReadOnlyList<ServerInfo> Find();
	}

	public class ServerInfoStore : IServerInfoStore
	{
		public ServerInfoStore(IOptions<Settings> options)
		{
			_options = options;
		}

		public IReadOnlyList<ServerInfo> Find()
		{
			return _options.Value.Servers
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


		private readonly IOptions<Settings> _options;
	}
}
