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
            status.Id = node.Element("id").Value.ToLong();
            status.Text = node.Element("text").ParseString();
            status.User = User.Create(node.Element("user"));
            status.CreatedAt = node.Element("created_at").Value.ToDateTime();
            status.Source = node.Element("source").ParseString();
            status.InReplyToStatusId = node.Element("in_reply_to_status_id").Value.ToLong();
            status.InReplyToUserId = node.Element("in_reply_to_user_id").Value.ToLong();
            status.InReplyToUserScreenName = node.Element("in_reply_to_screen_name").ParseString();
            status.RetweetedOriginal = Status.Create(node.Element("retweeted_status"));
            status.RetweetedCount = node.Element("retweeted_count").Value.ToLong();
            var urls = node.Element("entities").Element("urls");
            IEnumerable<XElement> urlElements = from el in urls.Elements("url") select el;
            foreach (XElement el in urlElements)
            {
                status.UrlEntities.Add(UrlEntity.Create(el));
            }
            var mentions = node.Element("entities").Element("user_mentions");
            IEnumerable<XElement> mentionElements = from el in mentions.Elements("user_mention") select el;
            foreach (XElement el in mentionElements)
            {
                status.UserMentionEntities.Add(UserMentionEntity.Create(el));
            }
            var hashtags = node.Element("entities").Element("hashtags");
            IEnumerable<XElement> hashtagElements = from el in hashtags.Elements("hashtag") select el;
            foreach (XElement el in hashtagElements)
            {
                status.hashtagEntities.Add(HashtagEntity.Create(el));
            }
			return status;
		}

		public long Id { get; private set; }
		public string Text { get; private set; }
		public User User { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public ICollection<UrlEntity> UrlEntities { get; private set; }
		public ICollection<UserMentionEntity> UserMentionEntities { get; private set; }
		public ICollection<HashtagEntity> hashtagEntities { get; private set; }
		public string Source { get; private set; }
		public long InReplyToStatusId { get; private set; }
		public long InReplyToUserId { get; private set; }
		public string InReplyToUserScreenName { get; private set; }
		public Status RetweetedOriginal { get; private set; }
        public long RetweetedCount { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is Status)
                return Id == ((Status)obj).Id;
            else 
                return false;
        }

        public override string ToString()
        {
            var status = this;
            if (RetweetedOriginal.Id > 0)
            {
                status = RetweetedOriginal;
            }
            return status.User.ScreenName + ":" + status.Text 
                + " [http://twitter.com/" +status.User.ScreenName + "/status/" + status.Id + "]";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
	}
}
