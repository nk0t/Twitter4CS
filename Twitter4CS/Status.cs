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
			return status;
		}


		public long Id { get; private set; }
		public string Text { get; private set; }
		public User User { get; private set; }
		public DateTime CreatedAt { get; private set; }
        public UrlEntity[] UrlEntities { get; private set; }
        public UserMentionEntity[] UserMentionsEntities { get; private set; }
        public HashTagEntity[] hashTagEntities { get; private set; }
		public string Source { get; private set; }
		public long InReplyToStatusId { get; private set; }
		public long InReplyToUserId { get; private set; }
		public string InReplyToUserScreenName { get; private set; }
		public Status RetweetedOriginal { get; private set; }
	}
}
