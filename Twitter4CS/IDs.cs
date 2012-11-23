using System;
using System.Collections.Generic;

namespace Twitter4CS
{
	public class IDs
	{
		private IDs()
		{
		}

		public static IDs Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var ids = new IDs();
			ids.PreviousCursor = (long)root["previous_cursor"];
			foreach (var id in (long[])root["ids"])
			{
				ids.IDsList.Add(id);
			}
			ids.NextCursor = (long)root["next_cursor"];			
			return ids;
		}

		public long[] GetIDs()
		{
			return IDsList.ToArray();
		}

		private List<long> IDsList = new List<long>();
		public long NextCursor { get; private set;}
		public long PreviousCursor { get; private set;}
	}
}
