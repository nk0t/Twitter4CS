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

		public static string ParseString(this string s)
		{
			return s != null ? s.Replace("&lt;", "<").Replace("&gt;", ">") : null;
		}

		public static string UrlEncode(this string s)
		{
			return Net.Http.UrlEncode(s);
		}

		public static DateTime ToDateTime(this string s, string format = "ddd MMM d HH':'mm':'ss zzz yyyy")
		{
			return DateTime.ParseExact(s,
				format,
				System.Globalization.DateTimeFormatInfo.InvariantInfo,
				System.Globalization.DateTimeStyles.None);
		}

		public static string ToHyphenSeparatedShortDateString(this DateTime d)
		{
			return d.ToShortDateString().Replace('/', '-');
		}

		public static int ToInteger(this string s)
		{
			int i;
			return int.TryParse(s, out i) ? i : 0;
		}
	}
}
