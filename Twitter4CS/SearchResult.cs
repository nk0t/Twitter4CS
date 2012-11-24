using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter4CS
{
	public class SearchResult
	{
		private SearchResult() { }

		public static SearchResult Create(dynamic root)
		{
			if(root == null)
				throw new ArgumentNullException();
			var result = new SearchResult();
			result.SinceId = (long)root["since_id"];
			result.MaxId = (long)root["max_id"];
			result.CompletedIn = (double)root["completed_in"];
			result.Count = (int)root["count"];
			return result;
		}

		public long SinceId { get; private set; }
		public long MaxId { get; private set; }
		public double CompletedIn { get; private set; }
		public int Count { get; private set; }

	}
}
