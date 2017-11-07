using KrisHemenway.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace KrisHemenway.MinecraftMonitor
{
	public interface IMinecraftStatusService
	{
		MinecraftStatus GetStatus(ServerInfo serverInfo);
	}

	internal class MinecraftStatusService : IMinecraftStatusService
	{
		public MinecraftStatusService(ILogger<MinecraftStatusService> logger = null)
		{
			_logger = logger ?? new LoggerFactory().CreateLogger<MinecraftStatusService>();
		}

		public MinecraftStatus GetStatus(ServerInfo serverInfo)
		{
			try
			{
				using (var tcpClient = new TcpClient())
				{
					tcpClient.ConnectAsync(serverInfo.Host, serverInfo.Port).Wait();
					var stream = tcpClient.GetStream();

					stream.Write(BitConverter.GetBytes(0xfe), 0, 1);
					stream.Write(BitConverter.GetBytes(0x01), 0, 1);
					var response = new byte[512];
					stream.Read(response, 0, 512);
					return ParseMessage(response);
				}
			}
			catch(Exception e)
			{
				_logger.LogError(e.ToString());
				return MinecraftStatus.Default;
			}
		}

		private static MinecraftStatus ParseMessage(byte[] response)
		{
			var responseSections = GetSections(response);

			if(responseSections == null)
			{
				return null;
			}

			return new MinecraftStatus
			{
				MessageOfTheDay = responseSections[3],
				PlayersOnline = Convert.ToInt32(responseSections[4]),
				MaxPlayers = Convert.ToInt32(responseSections[5]),
				Available = true,
				StatusTime = DateTime.Now
			};
		}

		private static IList<string> GetSections(byte[] response)
		{
			var sections = Encoding.Unicode.GetString(response).Split("\u0000\u0000\u0000".ToCharArray());

			if(string.IsNullOrEmpty(sections[3]) || string.IsNullOrEmpty(sections[4]))
			{
				var decodedResponse = Encoding.UTF8.GetString(response);
				var responseParts = Regex.Replace(decodedResponse, "\0+$", "").Replace("\0", "").Split(new[] { '�' });

				return new List<string>
				{
					"",
					"",
					"",
					responseParts[1],
					responseParts[2],
					responseParts[3]
				};
			}

			return sections;

		}

		private readonly ILogger<MinecraftStatusService> _logger;
	}

	public class MinecraftStatusRequestException : Exception
	{
		public MinecraftStatusRequestException(ServerInfo serverInfo, Exception e)
			: base($"Failure while requesting #{serverInfo.Host}:#{serverInfo.Port}", e)
		{
		}
	}
}
