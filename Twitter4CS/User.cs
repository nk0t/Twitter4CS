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
		public static User Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var user = new User();
            user.Id = (long)node.Element("id");
            user.UserName = (string)node.Element("name");
            user.ScreenName = (string)node.Element("screen_name");
            user.Bio = (string)node.Element("description");
            user.Followers = (long)node.Element("followers_count");
            user.Followings = (long)node.Element("friends_count");
            user.Favorites = (long)node.Element("favourites_count");
            user.Listed = (long)node.Element("listed_count");
            user.Tweets = (long)node.Element("statuses_count");
            user.ProfileImage = (string)node.Element("profile_image_url");
            user.IsProtected = bool.Parse((string)node.Element("protected"));
            user.CreatedAt = ((string)node.Element("created_at")).ToDateTime(); //Util.Extensions
            user.Location = (string)node.Element("location");
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
	}
}
