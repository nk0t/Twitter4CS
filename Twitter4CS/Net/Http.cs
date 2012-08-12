using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Twitter4CS.Net
{
	public static class Http
	{
		public static string HttpGet(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			string result = "";
			WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					using (var reader = new StreamReader(stream))
					{
						result = reader.ReadToEnd();
					}
				}
			}
			return result;
		}

		public static string HttpPost(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
			WebRequest req = WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.ContentLength = data.Length;

			string result = "";

			using (var stream = req.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					using (var reader = new StreamReader(stream, Encoding.UTF8))
					{
						result = reader.ReadToEnd();
					}
				}
			}
			return result;
		}

		public static string JoinParameters(IEnumerable<KeyValuePair<string, string>> parameters)
		{
			StringBuilder result = new StringBuilder();
			bool first = true;
			foreach (var parameter in parameters)
			{
				if (first)
					first = false;
				else
					result.Append('&');
				result.Append(parameter.Key);
				result.Append('=');
				result.Append(parameter.Value);
			}
			return result.ToString();
		}
	}
}
