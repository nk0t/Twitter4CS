using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace Twitter4CS.Authentication
{
	public abstract class OAuth
	{
		public OAuth() : this(null, null) { }

		public OAuth(string token, string secret)
		{
			this.Token = token;
			this.Secret = secret;
		}

		#region access token

		public string GetRequestToken()
		{
			SortedDictionary<string, string> parameters = GenerateParameters("");
			string signature = GenerateSignature("", "GET", RequestTokenUrl, parameters);
			parameters.Add("oauth_signature", UrlEncode(signature));
			string response = HttpGet(RequestTokenUrl, parameters);
			Dictionary<string, string> dic = ParseResponse(response);
			return dic["oauth_token"];
		}

		public Uri GetAuthorizeUrl(out string reqToken)
		{
			reqToken = GetRequestToken();
			string url = AuthorizeUrl + "?oauth_token=" + reqToken;
			return new Uri(url);
		}

		public bool GetAccessToken(string token, string pin, out long userId, out string userScreenName)
		{
			SortedDictionary<string, string> parameters = GenerateParameters(token);
			parameters.Add("oauth_verifier", pin);
			string signature = GenerateSignature(this.Secret, "GET", AccessTokenUrl, parameters);
			parameters.Add("oauth_signature", UrlEncode(signature));
			string response = HttpGet(AccessTokenUrl, parameters);
			Dictionary<string, string> dic = ParseResponse(response);
			if (dic.ContainsKey("oauth_token") && dic.ContainsKey("oauth_token_secret") &&
				Int64.TryParse(dic["user_id"], out userId))
			{
				this.Token = dic["oauth_token"];
				this.Secret = dic["oauth_token_secret"];
				userScreenName = dic["screen_name"];
				return true;
			}
			else
			{
				userId = 0;
				userScreenName = string.Empty;
				return false;
			}
		}

		#endregion

		#region util

		public string Get(string url, IDictionary<string, string> parameters)
		{
			SortedDictionary<string, string> parameters2 = GenerateParameters(this.Token);
			foreach (var p in parameters)
				parameters2.Add(p.Key, p.Value);
			string signature = GenerateSignature(this.Secret, "GET", url, parameters2);
			parameters2.Add("oauth_signature", UrlEncode(signature));
			return HttpGet(url, parameters2);
		}

		public string Post(string url, IDictionary<string, string> parameters)
		{
			SortedDictionary<string, string> parameters2 = GenerateParameters(this.Token);
			foreach (var p in parameters)
				parameters2.Add(p.Key, p.Value);
			string signature = GenerateSignature(this.Secret, "POST", url, parameters2);
			parameters2.Add("oauth_signature", UrlEncode(signature));
			return HttpPost(url, parameters2);
		}

		private string HttpGet(string url, IDictionary<string, string> parameters)
		{
			string result = "";
			WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
			using (WebResponse res = req.GetResponse())
			{
				using (Stream stream = res.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						result = reader.ReadToEnd();
					}
				}
			}
			return result;
		}

		string HttpPost(string url, IDictionary<string, string> parameters)
		{
			byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
			WebRequest req = WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.ContentLength = data.Length;

			string result = "";

			using (var reqStream = req.GetRequestStream())
			{
				reqStream.Write(data, 0, data.Length);
			}
			using (var res = req.GetResponse())
			{
				using (var resStream = res.GetResponseStream())
				{
					using (var reader = new StreamReader(resStream, Encoding.UTF8))
					{
						result = reader.ReadToEnd();
					}
				}
			}
			return result;

		}

		private Dictionary<string, string> ParseResponse(string response)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			foreach (string s in response.Split('&'))
			{
				int index = s.IndexOf('=');
				if (index == -1)
					result.Add(s, "");
				else
					result.Add(s.Substring(0, index), s.Substring(index + 1));
			}
			return result;
		}

		private string JoinParameters(IDictionary<string, string> parameters)
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

		private string GenerateSignature(string tokenSecret, string httpMethod, string url, SortedDictionary<string, string> parameters)
		{
			string signatureBase = GenerateSignatureBase(httpMethod, url, parameters);
			using (HMACSHA1 hmacsha1 = new HMACSHA1())
			{
				hmacsha1.Key = Encoding.ASCII.GetBytes(UrlEncode(ConsumerSecret) + '&' + UrlEncode(tokenSecret));
				byte[] data = System.Text.Encoding.ASCII.GetBytes(signatureBase);
				byte[] hash = hmacsha1.ComputeHash(data);
				return Convert.ToBase64String(hash);
			}
		}

		private string GenerateSignatureBase(string httpMethod, string url, SortedDictionary<string, string> parameters)
		{
			StringBuilder result = new StringBuilder();
			result.Append(httpMethod);
			result.Append('&');
			result.Append(UrlEncode(url));
			result.Append('&');
			result.Append(UrlEncode(JoinParameters(parameters)));
			return result.ToString();
		}

		private SortedDictionary<string, string> GenerateParameters(string token)
		{
			SortedDictionary<string, string> result = new SortedDictionary<string, string>();
			result.Add("oauth_consumer_key", this.ConsumerKey);
			result.Add("oauth_signature_method", "HMAC-SHA1");
			result.Add("oauth_timestamp", GenerateTimestamp());
			result.Add("oauth_nonce", GenerateNonce());
			result.Add("oauth_version", "1.0");
			if (!string.IsNullOrEmpty(token))
				result.Add("oauth_token", token);
			return result;
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

		private string GenerateNonce()
		{
			return Guid.NewGuid().ToString("N");
		}

		private string GenerateTimestamp()
		{
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}

		#endregion

		public string Token { get; set; }
		public string Secret { get; set; }

		protected abstract string ConsumerKey { get; }
        protected abstract string ConsumerSecret { get; }

		public const string RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
		public const string AuthorizeUrl = "https://api.twitter.com/oauth/authorize";
		public const string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";

		public const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
	}
}
