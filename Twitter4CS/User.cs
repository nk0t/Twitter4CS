using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class User
	{
        private User()
        { 
        }

		public static User Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var user = new User();
            user.Id = ((string)node.Element("id").Value).ToLong();
            user.UserName = (string)node.Element("name").Value;
            user.ScreenName = (string)node.Element("screen_name").Value;
            user.Bio = (string)node.Element("description").Value;
            user.Followers = ((string)node.Element("followers_count").Value).ToLong();
            user.Followings = ((string)node.Element("friends_count").Value).ToLong();
            user.Favorites = ((string)node.Element("favourites_count").Value).ToLong();
            user.Listed = ((string)node.Element("listed_count").Value).ToLong();
            user.Tweets = ((string)node.Element("statuses_count").Value).ToLong();
            user.ProfileImage = (string)node.Element("profile_image_url").Value;
            user.IsProtected = ((string)node.Element("protected").Value).ToBool();
            user.CreatedAt = ((string)node.Element("created_at").Value).ToDateTime(); //Util.Extensions
            user.Location = (string)node.Element("location").Value;
			return user;
		}

		public string UserName { get; private set; }
		public string ScreenName { get; private set; }
		public long Id { get; private set; }
		public string Bio { get; private set; }
		public long Followers { get; private set; }
		public long Followings { get; private set; }
		public long Favorites { get; private set; }
		public long Listed { get; private set; }
		public long Tweets { get; private set; }
		public string ProfileImage { get; private set; }
		public bool IsProtected { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public string Location { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is User)
                return Id == ((User)obj).Id;
            else
                return false;
        }

        public override string ToString()
        {
            return ScreenName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
	}
}
