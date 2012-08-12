using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter4CS.Authentication;
using Twitter4CS.Net;

namespace Twitter4CS.Rest
{
	public static class TwitterApi
	{
		public static string TwitterApiUrl = "https://api.twitter.com/1/";

		#region Status

		public static void UpdateStatus(this OAuth oauth, string text, long? inReplyTo = null)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("status", Http.UrlEncode(text)));
			if (inReplyTo != null && inReplyTo.HasValue)
			{
				param.Add(new KeyValuePair<string, string>("in_reply_to_status_id", inReplyTo.Value.ToString()));
			}
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));

			var url = TwitterApiUrl + "statuses/update.xml";
			oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
		}

		#endregion
	}
}
