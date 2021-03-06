﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Twitter4CS.Net;
using Twitter4CS.Rest;

namespace Twitter4CS.Authentication
{
	public abstract class OAuth
	{
		public OAuth() : this(null, null) { }

		public OAuth(string token, string secret)
		{
			this.Token = token;
			this.Secret = secret;
			this.UserId = 0;
			this.ScreenName = string.Empty;
			if (token != null && secret != null)
			{
				var user = this.GetOwnUser();
				this.UserId = user.Id;
				this.ScreenName = user.ScreenName;
			}
		}

		#region access token

		public string GetRequestToken()
		{
			SortedDictionary<string, string> parameters = GenerateOAuthParameters("");
			string signature = GenerateSignature("", "GET", RequestTokenUrl, parameters);
			parameters.Add("oauth_signature", Http.UrlEncode(signature));
			string response = Http.HttpGet(RequestTokenUrl, parameters).ToString();
			Dictionary<string, string> dic = ParseResponse(response);
			return dic["oauth_token"];
		}

		public Uri GetAuthorizeUrl(out string reqToken)
		{
			reqToken = GetRequestToken();
			string url = AuthorizeUrl + "?oauth_token=" + reqToken;
			return new Uri(url);
		}

		public bool GetAccessToken(string token, string pin)
		{
			try
			{
				long userId = 0;
				var parameters = GenerateOAuthParameters(token);
				parameters.Add("oauth_verifier", pin);
				string signature = GenerateSignature(this.Secret, "GET", AccessTokenUrl, parameters);
				parameters.Add("oauth_signature", Http.UrlEncode(signature));
				string response = Http.HttpGet(AccessTokenUrl, parameters).ToString();
				Dictionary<string, string> dic = ParseResponse(response);
				if (dic.ContainsKey("oauth_token") && dic.ContainsKey("oauth_token_secret") &&
					Int64.TryParse(dic["user_id"], out userId))
				{
					this.Token = dic["oauth_token"];
					this.Secret = dic["oauth_token_secret"];
					this.ScreenName = dic["screen_name"];
					this.UserId = userId;
					return true;
				}
				else
				{
					this.UserId = 0;
					this.ScreenName = string.Empty;
					return false;
				}
			}
			catch
			{
				this.UserId = 0;
				this.ScreenName = string.Empty;
				return false;
			}
		}

		#endregion

		#region util

		public enum RequestMethod
		{
			GET, POST
		}

		public dynamic RequestAPI(string uri, RequestMethod method, IEnumerable<KeyValuePair<string, string>> parameter)
		{
			if (method == RequestMethod.GET)
			{
				return RequestGet(uri, parameter);
			}
			else if (method == RequestMethod.POST)
			{
				return RequestPost(uri, parameter);
			}
			return new Util.DynamicJson();
		}

		public dynamic RequestGet(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			var parameters2 = GenerateOAuthParameters(this.Token);
			foreach (var p in parameters)
				parameters2.Add(p.Key, p.Value);
			string signature = GenerateSignature(this.Secret, "GET", url, parameters2);
			parameters2.Add("oauth_signature", Http.UrlEncode(signature));
			return Http.HttpGetJson(url, parameters2);
		}

		public dynamic RequestPost(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			var parameters2 = GenerateOAuthParameters(this.Token);
			foreach (var p in parameters)
				parameters2.Add(p.Key, p.Value);
			string signature = GenerateSignature(this.Secret, "POST", url, parameters2);
			parameters2.Add("oauth_signature", Http.UrlEncode(signature));
			return Http.HttpPostJson(url, parameters2);
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

		private string GenerateSignature(string tokenSecret, string httpMethod, string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			string signatureBase = GenerateSignatureBase(httpMethod, url, parameters);
			using (HMACSHA1 hmacsha1 = new HMACSHA1())
			{
				hmacsha1.Key = Encoding.ASCII.GetBytes(Http.UrlEncode(ConsumerSecret) + '&' + Http.UrlEncode(tokenSecret));
				byte[] data = System.Text.Encoding.ASCII.GetBytes(signatureBase);
				byte[] hash = hmacsha1.ComputeHash(data);
				return Convert.ToBase64String(hash);
			}
		}

		private string GenerateSignatureBase(string httpMethod, string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			StringBuilder result = new StringBuilder();
			result.Append(httpMethod);
			result.Append('&');
			result.Append(Http.UrlEncode(url));
			result.Append('&');
			result.Append(Http.UrlEncode(Http.JoinParameters(parameters)));
			return result.ToString();
		}

		private SortedDictionary<string, string> GenerateOAuthParameters(string token)
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

		public long? UserId { get; private set; }
		public string ScreenName { get; private set; }
		public string Token { get; set; }
		public string Secret { get; set; }

		protected abstract string ConsumerKey { get; }
		protected abstract string ConsumerSecret { get; }

		public const string RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
		public const string AuthorizeUrl = "https://api.twitter.com/oauth/authorize";
		public const string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";

	}
}
