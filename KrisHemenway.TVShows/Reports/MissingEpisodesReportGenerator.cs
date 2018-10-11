using KrisHemenway.TVShows.Shows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows.Reports
{
	public class MissingEpisodesReportGenerator
	{
		public IReadOnlyList<string> GenerateReport()
		{
			return new ShowStore()
				.FindAll().SelectMany(x => x.Episodes)
				.Where(x => !string.IsNullOrEmpty(x.VideoPath) && x.AirDate < DateTime.Today)
				.Select(x => x.ToString())
				.ToList();
		}
	}
}
