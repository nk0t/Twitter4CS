using System;
using Twitter4CS.Util;
using System.Collections;

namespace Twitter4CS
{
	/// <summary>
	/// 他のユーザーと自分との簡単な関係情報を提供します
	/// </summary>
	public class Friendship
	{
		private Friendship()
		{
		}

		public static Friendship Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var friendship = new Friendship();
			friendship.ScreenName = root["screen_name"];
			friendship.UserName = ((string)root["name"]).ParseString();
			friendship.UserId = (long)root["id"];
			var connections = (string[])root["connections"];
			if (Array.IndexOf<string>(connections, "following") > -1)
				friendship.IsFollowing = true;
			if (Array.IndexOf<string>(connections, "followed_by") > -1)
				friendship.IsFollowedBy = true;
			if (Array.IndexOf<string>(connections, "following_requested") > -1)
				friendship.IsFollowingRequested = true;
			return friendship;
		}

		public string ScreenName { get; private set; }
		public string UserName { get; private set; }
		public long UserId { get; private set; }
		public bool IsFollowing { get; private set; }
		public bool IsFollowedBy { get; private set; }
		public bool IsFollowingRequested { get; private set; }
	}
}
