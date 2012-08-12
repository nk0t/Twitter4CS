using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Twitter4CS.Util
{
	public static class Extensions
	{
		public static bool ToBool(this string s)
		{
			return s.ToLower() == "true";
		}

		public static long ToLong(this string s)
		{
			long l;
			return long.TryParse(s, out l) ? l : 0;
		}

		public static long ToLong(this XElement e)
		{
			return ToLong(e != null ? e.Value : null);
		}

		public static string ParseString(this XElement e)
		{
			return e != null ? e.Value.Replace("&lt;", "<").Replace("&gt;", ">") : null;
		}
	}
}
