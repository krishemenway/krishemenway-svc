using KrisHemenway.TVShowsCore.Episodes;
using System;
using System.Collections.Generic;

namespace KrisHemenway.TVShowsCore.Seriess
{
	public class Series
	{
		public Series()
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