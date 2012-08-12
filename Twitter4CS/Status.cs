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


		public long Id { get; set; }
		public string Text { get; set; }
		public User User { get; set; }
		public DateTime CreatedAt { get; set; }
		public object[] Entities { get; set; }
		public string Source { get; set; }
		public long InReplyToStatusId { get; set; }
		public long InReplyToUserId { get; set; }
		public string InReplyToUserScreenName { get; set; }
		public Status RetweetedOriginal { get; set; }
	}
}
