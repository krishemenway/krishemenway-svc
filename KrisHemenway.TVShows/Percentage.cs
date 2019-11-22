using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.TVShows
{
	public class Percentage
	{
		public Percentage(int count, int total)
		{
			Count = count;
			Total = total;
		}

		public double Value => (double) Count / Total * 100;

		public int Count { get; set; }
		public int Total { get; set; }

		public override string ToString()
		{
			return $"{Value.ToString("G2")} %";
		}

		public override bool Equals(object obj)
		{
			return Value.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static Percentage operator +(Percentage percentageOne, Percentage percentageTwo)
		{
			return new Percentage(percentageOne.Count + percentageTwo.Count, percentageOne.Total + percentageTwo.Total);
		}
	}

	public static class PercentageExtensions
	{
		public static Percentage CreatePercentageOf<T>(this IEnumerable<T> collection, Func<T, bool> calculatePercentageWithFilter)
		{
			return new Percentage(collection.Count(calculatePercentageWithFilter), collection.Count());
		}
	}
}
