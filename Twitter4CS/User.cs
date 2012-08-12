using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Twitter4CS
{
	public class User
	{
		public static User Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var user = new User();
			return user;
		}

		public string UserName { get; set; }
		public string ScreenName { get; set; }
		public long NumericId { get; set; }
		public string Bio { get; set; }
		public long Followers { get; set; }
		public long Followings { get; set; }
		public long Favorites { get; set; }
		public long Listed { get; set; }
		public long Tweets { get; set; }
		public string ProfileImage { get; set; }
		public string IsProtected { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Location { get; set; }
	}
}
