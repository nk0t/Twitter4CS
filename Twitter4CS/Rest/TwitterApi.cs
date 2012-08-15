using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
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

		#region Favorite

		public static Status Favorite(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("favorites/create/{0}.xml", statusId);
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(xdoc.Root);
		}

		public static Status UnFavorite(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("favorites/destroy/{0}.xml", statusId);
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(xdoc.Root);
		}

		#endregion

		public static Status Retweet(this OAuth oauth, long statusId)
		{
			var param = new List<KeyValuePair<string, string>>();
			param.Add(new KeyValuePair<string, string>("include_entities", "true"));
			var url = TwitterApiUrl + string.Format("statuses/retweet/{0}.xml", statusId);
			XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
			return Status.Create(xdoc.Root);
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
            var url = TwitterApiUrl + "users/show.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            return User.Create(xdoc.Root);
        }

        /// <summary>
        /// 指定した複数のScreenNameから最大100件までユーザーを取得
        /// </summary>
        public static User[] GetUsers(this OAuth oauth, string[] screenNames)
        {
            var param = new List<KeyValuePair<string, string>>();
            var names = string.Join(",", screenNames);
            param.Add(new KeyValuePair<string, string>("screen_name", names));
            param.Add(new KeyValuePair<string, string>("include_entities", "true"));
            var url = TwitterApiUrl + "users/lookup.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            IEnumerable<XElement> elements = from el in xdoc.Root.Elements("user") select el;
            ICollection<User> users = new List<User>();
            foreach (XElement el in elements)
            {
                users.Add(User.Create(el));
            }
            return users.ToArray();
        }

        /// <summary>
        /// 指定したUserIDからユーザーを取得
        /// </summary>
        public static User GetUser(this OAuth oauth, long userID)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("user_id", userID.ToString()));
            param.Add(new KeyValuePair<string, string>("include_entities", "true"));
            var url = TwitterApiUrl + "users/show.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            return User.Create(xdoc.Root);
        }

        /// <summary>
        /// 指定した複数のUserIDから最大100件までユーザーを取得
        /// </summary>
        public static User[] GetUsers(this OAuth oauth, long[] userID)
        {
            var param = new List<KeyValuePair<string, string>>();
            var names = string.Join(",", userID.ToString());
            param.Add(new KeyValuePair<string, string>("user_id", names));
            param.Add(new KeyValuePair<string, string>("include_entities", "true"));
            var url = TwitterApiUrl + "users/lookup.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            IEnumerable<XElement> elements = from el in xdoc.Root.Elements("user") select el;
            ICollection<User> users = new List<User>();
            foreach (XElement el in elements)
            {
                users.Add(User.Create(el));
            }
            return users.ToArray();
        }
        #endregion

        #region Friendships

        /// <summary>
        /// 指定したScreenNameのユーザーをフォローします 
        /// 失敗した場合WebExceptionが発生します
        /// </summary>
        /// <exception cref="WebException"/>
        public static User Follow(this OAuth oauth, string screenName)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("screen_name", screenName));
            param.Add(new KeyValuePair<string, string>("include_entities", "true"));
            var url = TwitterApiUrl + "friendships/create.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
            return User.Create(xdoc.Root);
        }

        /// <summary>
        /// 指定したScreenNameのユーザーへのフォローを解除します 
        /// 失敗した場合WebExceptionが発生します
        /// </summary>
        /// <exception cref="WebException"/>
        public static User Remove(this OAuth oauth, string screenName)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("screen_name", screenName));
            param.Add(new KeyValuePair<string, string>("include_entities", "true"));
            var url = TwitterApiUrl + "friendships/destroy.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.POST, param);
            return User.Create(xdoc.Root);
        }

        /// <summary>
        /// 指定したユーザー間の関係を取得します
        /// </summary>
        public static Relationship GetRelationship(this OAuth oauth, string sourceScreenName, string targetScreenName)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("source_screen_name",sourceScreenName));
            param.Add(new KeyValuePair<string, string>("target_screen_name", targetScreenName));
            var url = TwitterApiUrl + "friendships/destroy.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            return Relationship.Create(xdoc.Root);
        }
        #endregion

        #region IDs

        /// <summary>
        /// 指定したユーザーがフォローしているユーザーのIDを5000件まで取得します\n
        ///cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます
        /// </summary>
        public static IDs GetFriendIDs(this OAuth oauth, string screenName, long cursor = -1)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("screen_name", screenName));
            param.Add(new KeyValuePair<string, string>("cursor", cursor.ToString()));
            var url = TwitterApiUrl + "friends/ids.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            return IDs.Create(xdoc.Root);
        }

        /// <summary>
        /// 指定したユーザーをフォローしているユーザーのIDを5000件まで取得します\n
        ///cursorは-1から始まり、取得しきれなかった場合は次のcursorが与えられます
        /// </summary>
        public static IDs GetFollowerIDs(this OAuth oauth, string screenName, long cursor = -1)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("screen_name", screenName));
            param.Add(new KeyValuePair<string, string>("cursor", cursor.ToString()));
            var url = TwitterApiUrl + "followers/ids.xml";
            XDocument xdoc = oauth.RequestAPI(url, OAuth.RequestMethod.GET, param);
            return IDs.Create(xdoc.Root);
        }
        #endregion
    }
}
