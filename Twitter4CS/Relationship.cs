using System;

namespace Twitter4CS
{
	/// <summary>
	/// 二人のユーザー間の詳細な関係情報を提供します
	/// </summary>
	public class Relationship
	{
		private Relationship()
		{
		}

		public static Relationship Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var relation = new Relationship();
			root = root["relationship"];
			var source = root["source"];
			var target = root["target"];
			relation.SourceId = (long)source["id"];
			relation.SourceScreenName = source["screen_name"];
			relation.TargetId = (long)target["id"];
			relation.TargetScreenName = target["screen_name"];
			relation.IsSourceFollowingTarget = (bool)source["following"];
			relation.IsSourceFollowedByTarget = (bool)source["followed_by"];
			relation.IsSourceBlockingTarget = (bool)source["blocking"];
			relation.IsSourceMarkingSpamTarget = (bool)source["marked_spam"];
			relation.IsSourceNotificationEnabled = (bool)source["notifications_enabled"];
			return relation;
		}

		public long SourceId { get; private set; }
		public string SourceScreenName { get; private set; }
		public long TargetId { get; private set; }
		public string TargetScreenName { get; private set; }
		public bool IsSourceFollowedByTarget { get; private set; }
		public bool IsSourceFollowingTarget { get; private set; }
		public bool IsSourceBlockingTarget { get; private set; }
		public bool IsSourceMarkingSpamTarget { get; private set; }
		public bool IsSourceNotificationEnabled { get; private set; }
	}
}
