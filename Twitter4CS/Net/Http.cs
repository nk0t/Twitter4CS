using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Twitter4CS.Util;

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

		public static XDocument HttpGetXml(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			XDocument xml;
			WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					xml = XDocument.Load(stream);
				}
			}
			return xml;
		}

		public static dynamic HttpGetJson(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			dynamic json;
			WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					json = DynamicJson.Parse(stream);
				}
			}
			return json;
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

		public static XDocument HttpPostXml(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
			WebRequest req = WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.ContentLength = data.Length;

			XDocument xml;

			using (var stream = req.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					xml = XDocument.Load(stream);
				}
			}
			return xml;
		}

		public static dynamic HttpPostJson(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
			WebRequest req = WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.ContentLength = data.Length;

			using (var stream = req.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			using (var res = req.GetResponse())
			{
				using (var stream = res.GetResponseStream())
				{
					return DynamicJson.Parse(stream);
				}
			}
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

		public static string UrlEncode(string value, Encoding encoding, bool isUpper = true)
		{
			if (value == null)
				value = "";
			StringBuilder result = new StringBuilder();
			byte[] data = encoding.GetBytes(value);
			foreach (byte b in data)
			{
				if (b < 0x80 && AllowedChars.IndexOf((char)b) != -1)
				{
					result.Append((char)b);
				}
				else
				{
					if (isUpper)
						result.Append('%' + String.Format("{0:X2}", (int)b));
					else
						result.Append('%' + String.Format("{0:x2}", (int)b));
				}
			}
			return result.ToString();
		}

		public static string UrlEncode(string value)
		{
			return UrlEncode(value, Encoding.UTF8);
		}

		public const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
	}
}
