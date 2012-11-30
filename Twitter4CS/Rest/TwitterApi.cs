using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Twitter4CS.Authentication;
using Twitter4CS.Net;
using Twitter4CS.Util;

namespace Twitter4CS.Rest
{
	public static class TwitterApi
	{
		public static string TwitterApiUrl = "https://api.twitter.com/1.1/";

		#region Status

		/// <summary>
		/// 指定したStatusIdのステータスを取得します
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
		/// 指定したStatusIdのステータスを削除します
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
		/// ツイートを投稿します
		/// </summary>
		/// <param name="text">投稿する文字列</param>
		/// <param name="inReplyTo">リプライ先のStatusId</param>
		/// <returns></returns>
		public static Status UpdateStatus(this OAuth oauth, string text, long? inReplyTo = null)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("status", text.UrlEncode()));
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

		#region Timeline

		/// <summary>
		/// Mentionsを取得します
		/// </summary>
		public static Status[] GetMentionsTimeline(this OAuth oauth, int count = 200, int sinceId = -1, int maxId = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("count", count.ToString()));
			if (sinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", sinceId.ToString()));
			if(maxId > 0)
				param.Add(new KeyValuePair<string,string>("max_id", maxId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "statuses/mentions_timeline.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<Status>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(Status.Create(el));
			}
			return list.ToArray();
		}

		/// <summary>
		/// <para>指定したScreenNameのユーザーの発言を取得します</para>
		/// <para>指定しなければ自分の発言を取得します</para>
		/// </summary>
		public static Status[] GetUserTimeline(this OAuth oauth, string screenName = null, int count = 200, int sinceId = -1, int maxId = -1, bool includeRts = false)
		{
			var param = new List<KeyValuePair<string, string>>();
			if(screenName != null)
				param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("count", count.ToString()));
			if (sinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", sinceId.ToString()));
			if (maxId > 0)
				param.Add(new KeyValuePair<string, string>("max_id", maxId.ToString()));
			if (includeRts)
				param.Add(new KeyValuePair<string, string>("include_rts", includeRts.ToString()));
			var url = TwitterApiUrl + "statuses/user_timeline.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<Status>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(Status.Create(el));
			}
			return list.ToArray();
		}

		/// <summary>
		/// 自分のタイムラインを取得します
		/// </summary>
		public static Status[] GetHomeTimeline(this OAuth oauth, int count = 200, int sinceId = -1, int maxId = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("count", count.ToString()));
			if (sinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", sinceId.ToString()));
			if (maxId > 0)
				param.Add(new KeyValuePair<string, string>("max_id", maxId.ToString()));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "statuses/home_timeline.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<Status>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(Status.Create(el));
			}
			return list.ToArray();
		}

		#endregion

		#region Favorite

		/// <summary>
		/// 指定したStatusIdからツイートをお気に入りに追加します
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
		/// 指定したStatusIdからツイートをお気に入りから削除します
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

		#region Retweet

		/// <summary>
		/// 指定したStatusIdからツイートをリツイートします
		/// </summary>
		public static Status Retweet(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/retweet/{0}.json", statusId);
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(json);
		}

		#endregion

		#region User

		/// <summary>
		/// 自分自身のUserオブジェクトを取得します
		/// </summary>
		public static User GetOwnUser(this OAuth oauth)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "false"));
			var url = TwitterApiUrl + "account/verify_credentials.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定したScreenNameからユーザーを取得します
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
		/// 指定した複数のScreenNameから最大100件までユーザーを取得します
		/// </summary>
		public static User[] GetUsers(this OAuth oauth, string[] screenNames)
		{
			var param = new List<KeyValuePair<string, string>>();
			var names = string.Join(",", screenNames);
			param.Add(new KeyValuePair<string, string>("screen_name", names.UrlEncode()));
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
		/// 指定したUserIDからユーザーを取得します
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
		/// 指定した複数のUserIDから最大100件までユーザーを取得します
		/// </summary>
		public static User[] GetUsers(this OAuth oauth, long[] userId)
		{
			var param = new List<KeyValuePair<string, string>>();
			var ids = string.Join(",", userId.ToString());
			param.Add(new KeyValuePair<string, string>("user_id", ids.UrlEncode()));
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

		/// <summary>
		/// <para>指定したユーザーと自分との簡単な関係を取得します</para>
		/// <para>詳細な関係が知りたければGetRelationshipを使用してください</para>
		/// </summary>
		public static Friendship[] GetFriendships(this OAuth oauth, string[] targetScreenNames)
		{
			var param = new List<KeyValuePair<string, string>>();
			var names = string.Join(",", targetScreenNames);
			param.Add(new KeyValuePair<string, string>("screen_name", names.UrlEncode()));
			var url = TwitterApiUrl + "friendships/lookup.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			var list = new List<Friendship>();
			foreach (var el in (dynamic[])json)
			{
				list.Add(Friendship.Create(el));
			}
			return list.ToArray();
		}
		#endregion

		#region Block and Spam

		/// <summary>
		/// 指定したScreenNameのユーザーをブロックします 
		/// </summary>
		public static User Block(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "blocks/create.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定したScreenNameのユーザーへのブロックを解除します 
		/// </summary>
		public static User Unblock(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + "blocks/destroy.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return User.Create(json);
		}

		/// <summary>
		/// 指定したScreenNameのユーザーをスパム報告します 
		/// </summary>
		public static User ReportSpam(this OAuth oauth, string screenName)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("screen_name", screenName));
			var url = TwitterApiUrl + "users/report_spam.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return User.Create(json);
		}

		#endregion

		#region IDs

		/// <summary>
		/// <para>指定したユーザーがフォローしているユーザーのIDを5000件まで取得します</para>
		/// <para>cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます</para>
		/// </summary>
		public static IDs GetBlockingIDs(this OAuth oauth, long cursor = -1)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("cursor", cursor.ToString()));
			var url = TwitterApiUrl + "blocks/ids.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			return IDs.Create(json);
		}

		/// <summary>
		/// <para>指定したユーザーがフォローしているユーザーのIDを5000件まで取得します</para>
		/// <para>cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます</para>
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
		/// <para>指定したユーザーをフォローしているユーザーのIDを5000件まで取得します</para>
		/// <para>cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます</para>
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
			param.Add(new KeyValuePair<string, string>("text", text.UrlEncode()));
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
			param.Add(new KeyValuePair<string, string>("text", text.UrlEncode()));
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

		#region Search

		/// <summary>
		/// 指定した文字列でツイートを検索します
		/// </summary>
		/// <param name="query">検索したい文字列</param>
		/// <param name="resultStatuses">検索結果を格納するコレクション</param>
		/// <param name="prop">検索条件を定義したSearchPropertiesオブジェクト</param>
		/// <returns>検索結果を表すSearchResultオブジェクト</returns>
		public static SearchResult Search(this OAuth oauth, string query, out ICollection<Status> resultStatuses, SearchProperties prop = null)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("q", query.UrlEncode()));
			if (prop == null)
				prop = new SearchProperties();
			param.Add(new KeyValuePair<string, string>("count", prop.Count.ToString()));
			param.Add(new KeyValuePair<string, string>("lang", prop.Lang));
			param.Add(new KeyValuePair<string, string>("result_type", prop.ResultType.ToString()));
			if (prop.SinceId > 0)
				param.Add(new KeyValuePair<string, string>("since_id", prop.SinceId.ToString()));
			if (prop.MaxId > 0)
				param.Add(new KeyValuePair<string, string>("max_id", prop.MaxId.ToString()));
			if (prop.Until == new DateTime(0))
				param.Add(new KeyValuePair<string, string>("until", prop.Until.ToHyphenSeparatedShortDateString().UrlEncode()));
			var url = TwitterApiUrl + "search/tweets.json";
			var json = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
			resultStatuses = new List<Status>();
			foreach (var el in (dynamic[])json["statuses"])
			{
				resultStatuses.Add(Status.Create(el));
			}
			return SearchResult.Create(json["search_metadata"]);
		}

		#endregion

	}
}
