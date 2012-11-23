﻿using System;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class User
	{
		private User()
		{
		}

		public static User Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var user = new User();
			user.Id = (long)root["id"];
			user.UserName = ((string)root["name"]).ParseString();
			user.ScreenName = root["screen_name"];
			user.Bio = ((string)root["description"]).ParseString();
			user.Followers = (long)root["followers_count"];
			user.Followings = (long)root["friends_count"];
			user.Favorites = (long)root["favourites_count"];
			user.Listed = (long)root["listed_count"];
			user.Tweets = (long)root["statuses_count"];
			user.ProfileImage = root["profile_image_url"];
			user.IsProtected = (bool)root["protected"];
			user.CreatedAt = ((string)root["created_at"]).ToDateTime();
			user.Location = root["location"];
			user.LatestStatus = root.IsDefined("status") ? Status.Create(root["status"], user) : null;
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
		public Status LatestStatus { get; private set; }

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