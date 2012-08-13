using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class Status
	{
		private Status()
		{
		}

		public static Status Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var status = new Status();
			status.Id = node.Element("id").ToLong();
            status.User = User.Create(node.Element("user"));
			status.Text = node.Element("text").ParseString();
			status.Source = node.Element("source").ParseString();
			status.CreatedAt = node.Element("created_at").Value.ToDateTime();
            status.InReplyToStatusId = node.Element("in_reply_to_status_id").ToLong();
            status.InReplyToUserId = node.Element("in_reply_to_user_id").ToLong();
            status.InReplyToUserScreenName = node.Element("in_reply_to_screen_name").ParseString();
            status.Entities = Entity.Create(node.Element("entities"));
			return status;
		}


		public long Id { get; private set; }
		public string Text { get; private set; }
		public User User { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public Entity[] Entities { get; private set; }
		public string Source { get; private set; }
		public long InReplyToStatusId { get; private set; }
		public long InReplyToUserId { get; private set; }
		public string InReplyToUserScreenName { get; private set; }
		public Status RetweetedOriginal { get; private set; }
	}
}
