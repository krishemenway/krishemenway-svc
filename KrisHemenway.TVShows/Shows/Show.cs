using KrisHemenway.TVShows.Episodes;
using StronglyTyped.GuidIds;
using System;
using System.Collections.Generic;

namespace KrisHemenway.TVShows.Shows
{
	public interface IShow
	{
		Id<Show> ShowId { get; }
		string Name { get; }

		string Path { get; }
		int? MazeId { get; }

		IReadOnlyList<IEpisode> Episodes { get; }

		DateTime Created { get; }
		DateTime LastModified { get; }
	}

	public class Show : IShow
	{
		public Show()
		{
			Episodes = new List<Episode>();
		}

		public Id<Show> ShowId { get; set; }
		public string Name { get; set; }

		public string Path { get; set; }

		public int? MazeId { get; set; }

		public IReadOnlyList<IEpisode> Episodes { get; set; }

		public DateTime Created { get; set; }
		public DateTime LastModified { get; set; }
	}
}