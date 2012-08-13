﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter4CS.Authentication;
using Twitter4CS.Net;
using System.Xml.Linq;

namespace Twitter4CS.Rest
{
	public static class TwitterApi
	{
		public static string TwitterApiUrl = "https://api.twitter.com/1/";

		#region Status

		/// <summary>
		/// Statusを取得
		/// </summary>
		/// <param name="oauth"></param>
		/// <param name="statusId"></param>
		/// <returns></returns>
		public static Status GetStatus(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/show/{0}.xml", statusId);
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return Status.Create(xdoc.Root);
		}

		/// <summary>
		/// Statusを削除
		/// </summary>
		/// <param name="oauth"></param>
		/// <param name="statusId"></param>
		/// <returns></returns>
		public static Status DestroyStatus(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/destroy/{0}.xml", statusId);
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(xdoc.Root);
		}

		/// <summary>
		/// Statusを投稿
		/// </summary>
		/// <param name="oauth"></param>
		/// <param name="text"></param>
		/// <param name="inReplyTo"></param>
		/// <returns></returns>
		public static Status UpdateStatus(this OAuth oauth, string text, long? inReplyTo = null)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("status", Http.UrlEncode(text)));
			if (inReplyTo != null && inReplyTo.HasValue)
			{
				param.Add(new KeyValuePair<string, string>("in_reply_to_status_id", inReplyTo.Value.ToString()));
			}
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));

			var url = TwitterApiUrl + "statuses/update.xml";
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(xdoc.Root);
		}

		#endregion
	}
}