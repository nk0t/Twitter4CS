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

		public static bool ToBool(this XElement e)
		{
			return ToBool(e.Value);
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

		public static string ParseString(this string s)
		{
			return s != null ? s.Replace("&lt;", "<").Replace("&gt;", ">") : null;
		}

		public static DateTime ToDateTime(this string s, string format = "ddd MMM d HH':'mm':'ss zzz yyyy")
		{
			return DateTime.ParseExact(s,
				format,
				System.Globalization.DateTimeFormatInfo.InvariantInfo,
				System.Globalization.DateTimeStyles.None);
		}

		public static DateTime ToDateTime(this XElement e, string format = "ddd MMM d HH':'mm':'ss zzz yyyy")
		{
			return ToDateTime(e.Value, format);
		}

		public static int ToInteger(this string s)
		{
			int i;
			return int.TryParse(s, out i) ? i : 0;
		}

		public static int ToInteger(this XElement e)
		{
			return ToInteger(e.Value);
		}
	}
}
