using KrisHemenway.TVShows.Seriess;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System;

namespace KrisHemenway.TVShows.Shows
{
	public class ShowFileMonitor
	{
		public void MonitorShow(IShow show)
		{
			ChangeToken = _fileProvider.Watch("**/*.*");

			var task = new TaskCompletionSource<object>();
			ChangeToken.RegisterChangeCallback(OnChange, task);
		}

		private void OnChange(object obj)
		{
			throw new NotImplementedException();
		}

		private static PhysicalFileProvider _fileProvider = new PhysicalFileProvider(Program.Settings.ExecutablePath);

		private IChangeToken ChangeToken { get; set; }
	}
}
