using System;
using System.Collections.Generic;
using Twitter4CS.Util;

namespace Twitter4CS
{
	/// <summary>
	/// ステータスオブジェクトを表します
	/// </summary>
	public class Status
	{
		private Status()
		{
		}

		public static Status Create(dynamic root, User user = null)
		{
			if (root == null)
				throw new ArgumentNullException();
			var status = new Status();
			status.Id = (long)root["id"];
			status.Text = root["text"];
			status.User = user != null ? user : User.Create(root["user"]);
			status.CreatedAt = ((string)root["created_at"]).ToDateTime();
			status.Source = ((string)root["source"]).ParseString();
			status.InReplyToStatusId = root["in_reply_to_status_id"] == null ? 0 : (long)root["in_reply_to_status_id"];
			status.InReplyToUserId = root["in_reply_to_user_id"] == null ? 0 : (long)root["in_reply_to_user_id"];
			status.InReplyToUserScreenName = root["in_reply_to_screen_name"];
			status.RetweetedOriginal = root.IsDefined("retweeted_status") ? Status.Create(root["retweeted_status"]) : null;
			status.RetweetedCount = (long)root["retweet_count"];
			var entities = root["entities"];
			var urls = entities["urls"];
			status.UrlEntities = new List<UrlEntity>();
			foreach (var el in (dynamic[])urls)
			{
				status.UrlEntities.Add(UrlEntity.Create(el));
			}
			var mentions = entities["user_mentions"];
			status.UserMentionEntities = new List<UserMentionEntity>();
			foreach (var el in (dynamic[])mentions)
			{
				status.UserMentionEntities.Add(UserMentionEntity.Create(el));
			}
			var hashtags = entities["hashtags"];
			status.HashtagEntities = new List<HashtagEntity>();
			foreach (var el in (dynamic[])hashtags)
			{
				status.HashtagEntities.Add(HashtagEntity.Create(el));
			}
			return status;
		}

		public long Id { get; private set; }
		public string Text { get; private set; }
		public User User { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public ICollection<UrlEntity> UrlEntities { get; private set; }
		public ICollection<UserMentionEntity> UserMentionEntities { get; private set; }
		public ICollection<HashtagEntity> HashtagEntities { get; private set; }
		public string Source { get; private set; }
		public long InReplyToStatusId { get; private set; }
		public long InReplyToUserId { get; private set; }
		public string InReplyToUserScreenName { get; private set; }
		public Status RetweetedOriginal { get; private set; }
		public long RetweetedCount { get; private set; }

		public override bool Equals(object obj)
		{
			var status = obj as Status;
			if (status != null)
				return Id == status.Id;
			else 
				return false;
		}

		public override string ToString()
		{
			return string.Format("{0}:{1} [http://twitter.com/{0}/status/{2}]",User.ScreenName, Text, Id);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
	}
}
