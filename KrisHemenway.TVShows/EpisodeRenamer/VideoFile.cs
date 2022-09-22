using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KrisHemenway.TVShows.EpisodeRenamer
{
	public interface IVideoFile
	{
		string FileName { get; }
		string FileExtension { get; }

		string FullPath { get; }

		string DirectoryName { get; }
		string DirectoryPath { get; }

		IReadOnlyList<string> GetAllParentDirectoryPaths();
		IReadOnlyList<string> GetAllParentDirectories(IReadOnlyList<string> blacklist = null);
	}

	public class VideoFile : IVideoFile
	{
		public VideoFile(FileInfo file)
		{
			_fileInfo = file;
		}

		public string FileName
		{
			get { return _fileInfo.Name; }
		}

		public string FileExtension
		{
			get { return _fileInfo.Extension; }
		}

		public string FullPath
		{
			get { return _fileInfo.FullName; }
		}

		public string DirectoryName
		{
			get { return _fileInfo.DirectoryName; }
		}

		public string DirectoryPath
		{
			get { return _fileInfo.Directory.FullName; }
		}

		public IReadOnlyList<string> GetAllParentDirectoryPaths()
		{
			var directories = new List<string>();

			var currentDirectory = _fileInfo.Directory;
			do
			{
				directories.Add(currentDirectory.FullName);
				currentDirectory = currentDirectory.Parent;
			} while (currentDirectory.Parent != null);

			return directories;
		}

		public IReadOnlyList<string> GetAllParentDirectories(IReadOnlyList<string> blacklist = null)
		{
			var directories = new List<string>();
			blacklist ??= Array.Empty<string>();

			var currentDirectory = _fileInfo.Directory;

			while (currentDirectory != null)
			{
				if (!blacklist.Contains(currentDirectory.Name.ToLower(), StringComparer.CurrentCultureIgnoreCase))
				{
					directories.Add(currentDirectory.Name);
				}

				currentDirectory = currentDirectory.Parent;
			}

			return directories;
		}

		public override string ToString()
		{
			return FullPath;
		}

		private readonly FileInfo _fileInfo;
	}
}
