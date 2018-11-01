using System.IO;
using System.Collections.Generic;
using System.Linq;
using KrisHemenway.Common;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IVideoFileScanner
	{
		Result<IReadOnlyList<IVideoFile>> Scan(string path);
	}

	public class VideoFileScanner : IVideoFileScanner
	{
		public VideoFileScanner(ISettings settings = null)
		{
			_settings = settings ?? Program.Settings;
		}

		public Result<IReadOnlyList<IVideoFile>> Scan(string path)
		{
			if (File.Exists(path))
			{
				return Result<IReadOnlyList<IVideoFile>>.Successful(TryScanFile(path).ToList());
			}
			else if (Directory.Exists(path))
			{
				return Result<IReadOnlyList<IVideoFile>>.Successful(ScanDirectory(path).ToList());
			}
			else
			{
				return Result<IReadOnlyList<IVideoFile>>.Failure("The path {path} was not found");
			}
		}

		private IEnumerable<IVideoFile> TryScanFile(string filePath)
		{
			var file = new FileInfo(filePath);
			if (_settings.RenameVideoFileExtensions.Contains(file.Extension.Replace(".", "")))
			{
				yield return new VideoFile(file);
			}
		}

		private IEnumerable<IVideoFile> ScanDirectory(string directoryPath)
		{
			var directory = new DirectoryInfo(directoryPath);

			foreach (var file in directory.GetFiles())
			{
				foreach (var foundFile in TryScanFile(file.FullName))
				{
					yield return foundFile;
				}
			}

			foreach (var subDirectory in directory.GetDirectories())
			{
				foreach(var file in ScanDirectory(subDirectory.FullName))
				{
					yield return file;
				}
			}
		}

		private readonly ISettings _settings;
	}
}
