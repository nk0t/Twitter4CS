using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;


namespace Twitter4CS
{
	public class IDs
	{
		private IDs()
		{
		}

		public static IDs Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var ids = new IDs();
			IEnumerable<XElement> elements = from el in node.Element("ids").Elements("id") select el;
			foreach (XElement el in elements)
			{
				ids.IDsList.Add(el.ToLong());
			}
			ids.NextCursor = node.Element("next_cursor").ToLong();
			ids.PreviousCursor = node.Element("previous_cursor").ToLong();
			return ids;
		}

		public static IDs Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var ids = new IDs();
			ids.PreviousCursor = (long)root.previous_cursor;
			foreach (var id in root.ids)
			{
				ids.IDsList.Add((long)id);
			}
			ids.NextCursor = (long)root.next_cursor;			
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
