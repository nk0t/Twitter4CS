using System.Collections.Generic;
using System.Linq;
using System.Net;
using Twitter4CS.Authentication;
using Twitter4CS.Net;

namespace Twitter4CS.Rest
{
	public static class TwitterApi
	{
		public static string TwitterApiUrl = "https://api.twitter.com/1.1/";

		#region Status

		/// <summary>
		/// Statusを取得
		/// </summary>
		public static Status GetStatus(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("id", statusId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "statuses/show.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return Status.Create(json);
		}

		/// <summary>
		/// Statusを削除
		/// </summary>
		public static Status DestroyStatus(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/destroy/{0}.json", statusId);
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		/// <summary>
		/// Statusを投稿
		/// </summary>
		public static Status UpdateStatus(this OAuth oauth, string text, long? inReplyTo = null)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("status", Http.UrlEncode(text)));
			if (inReplyTo != null && inReplyTo.HasValue)
			{
				param.Add(new KeyValuePair<string, string>("in_reply_to_status_id", inReplyTo.Value.ToString()));
			}
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));

			var url = TwitterApiUrl + "statuses/update.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		#endregion

		#region Favorite

		/// <summary>
		/// 指定したStatusIdからツイートをお気に入りに追加する
		/// </summary>
		public static Status Favorite(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("id", statusId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "favorites/create.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		/// <summary>
		/// 指定したStatusIdからツイートをお気に入りから削除する
		/// </summary>
		public static Status UnFavorite(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("id", statusId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "favorites/destroy.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		#endregion

		/// <summary>
		/// 指定したStatusIdからツイートをリツイートする
		/// </summary>
		public static Status Retweet(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/retweet/{0}.json", statusId);
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		#region User

		/// <summary>
		/// 指定したScreenNameからユーザーを取得
		/// </summary>
		public static User GetUser(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "users/show.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定した複数のScreenNameから最大100件までユーザーを取得
		/// </summary>
		public static User[] GetUsers(this OAuth oauth, string[] screenNames)
		{
			var param = new List<KeyValuePair<string, string>>();
			var names = string.Join(",", screenNames);
			param.Add(new KeyValuePair<string, string>("screen_name", Http.UrlEncode(names)));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "users/lookup.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			ICollection<User> users = new List<User>();
			foreach (var user in (dynamic[])json)
			{
				users.Add(User.Create(user));
			}
			return users.ToArray();
		}

		/// <summary>
		/// 指定したUserIDからユーザーを取得
		/// </summary>
		public static User GetUser(this OAuth oauth, long userId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("user_id", userId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "users/show.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定した複数のUserIDから最大100件までユーザーを取得
		/// </summary>
		public static User[] GetUsers(this OAuth oauth, long[] userId)
		{
			var param = new List<KeyValuePair<string, string>>();
			var ids = string.Join(",", userId.ToString());
			param.Add(new KeyValuePair<string, string>("user_id", Http.UrlEncode(ids)));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "users/lookup.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			ICollection<User> users = new List<User>();
			foreach (var user in (dynamic[])json)
			{
				users.Add(User.Create(user));
			}
			return users.ToArray();
		}
		#endregion

		#region Friendships

		/// <summary>
		/// 指定したScreenNameのユーザーをフォローします 
		/// </summary>
		public static User Follow(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "friendships/create.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定したScreenNameのユーザーへのフォローを解除します 
		/// </summary>
		public static User Remove(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "friendships/destroy.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定したユーザー間の関係を取得します
		/// </summary>
		public static Relationship GetRelationship(this OAuth oauth, string sourceScreenName, string targetScreenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("source_screen_name", sourceScreenName));
			param.Add(new KeyValuePair<string, string>("target_screen_name", targetScreenName));
			var url = TwitterApiUrl + "friendships/show.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return Relationship.Create(json);
		}
		#endregion

		#region IDs

		/// <summary>
		/// 指定したユーザーがフォローしているユーザーのIDを5000件まで取得します\n
		/// cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます
		/// </summary>
		public static IDs GetFriendIDs(this OAuth oauth, string screenName, long cursor = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("cursor", cursor.ToString()));
			var url = TwitterApiUrl + "friends/ids.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return IDs.Create(json);
		}

		/// <summary>
		/// 指定したユーザーをフォローしているユーザーのIDを5000件まで取得します\n
		/// cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます
		/// </summary>
		public static IDs GetFollowerIDs(this OAuth oauth, string screenName, long cursor = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("cursor", cursor.ToString()));
			var url = TwitterApiUrl + "followers/ids.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return IDs.Create(json);
		}
		#endregion

		#region DirectMessage
		
		/// <summary>
		/// 受信したダイレクトメッセージを取得します
		/// </summary>
		public static DirectMessage[] GetRecievedDirectMessages(this OAuth oauth, int count = 20, int page = 1, int sinceId = -1, int maxId = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("count", count.ToString()));
			param.Add(new KeyValuePair<string, string>("page", page.ToString()));
			if (sinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", sinceId.ToString()));
			if (maxId > 0)
				param.Add(new KeyValuePair<string, string>("max_id", maxId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "direct_messages.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<DirectMessage>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(DirectMessage.Create(el));
			}
			return list.ToArray();
		}

		/// <summary>
		/// 送信したダイレクトメッセージを取得します
		/// </summary>
		public static DirectMessage[] GetSentDirectMessages(this OAuth oauth, int count = 20, int page = 1, int sinceId = -1, int maxId = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("count", count.ToString()));
			param.Add(new KeyValuePair<string, string>("page", page.ToString()));
			if (sinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", sinceId.ToString()));
			if (maxId > 0)
				param.Add(new KeyValuePair<string, string>("max_id", maxId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "direct_messages/sent.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<DirectMessage>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(DirectMessage.Create(el));
			}
			return list.ToArray();
		}

		/// <summary>
		/// 指定したScreenNameのユーザーにダイレクトメッセージを送信します
		/// </summary>
		public static DirectMessage SendDirectMessage(this OAuth oauth, string screenName, string text)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("text", Http.UrlEncode(text)));
			var url = TwitterApiUrl + "direct_messages/new.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return DirectMessage.Create(json);
		}

		/// <summary>
		/// 指定したUserIdのユーザーにダイレクトメッセージを送信します
		/// </summary>
		public static DirectMessage SendDirectMessage(this OAuth oauth, long userId, string text)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("user_id", userId.ToString()));
			param.Add(new KeyValuePair<string, string>("text", Http.UrlEncode(text)));
			var url = TwitterApiUrl + "direct_messages/new.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return DirectMessage.Create(json);
		}

		/// <summary>
		/// 指定したMessageIdのダイレクトメッセージを削除します
		/// </summary>
		public static DirectMessage DestroyDirectMessage(this OAuth oauth, long messageId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("id", messageId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "direct_messages/destroy.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return DirectMessage.Create(json);
		}
		#endregion
	}
}
