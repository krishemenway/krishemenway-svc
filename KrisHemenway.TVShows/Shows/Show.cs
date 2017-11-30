using KrisHemenway.TVShows.Episodes;
using System;
using System.Collections.Generic;

namespace KrisHemenway.TVShows.Shows
{
	public interface IShow
	{
		int Id { get; }
		string Name { get; }
		string Path { get; }

		int? RageId { get; }
		int? MazeId { get; }

		List<Episode> Episodes { get; }

		DateTime Created { get; }
		DateTime LastModified { get; }
	}

	public class Show : IShow
	{
		public Show()
		{
			Episodes = new List<Episode>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }

		public int? RageId { get; set; }
		public int? MazeId { get; set; }

		public List<Episode> Episodes { get; set; }

		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}