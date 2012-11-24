﻿using System;

namespace Twitter4CS
{
	public class UserMentionEntity : Entity
	{
		/// <summary>
		/// ユーザーメンション要素を表します
		/// </summary>
		private UserMentionEntity()
		{
		}

		public static UserMentionEntity Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var entity = new UserMentionEntity();
			entity.ScreenName = root["screen_name"];
			entity.Name = root["name"];
			entity.UserId = (long)root["id"];
			entity.StartIndex = ((int[])root["indices"])[0];
			entity.EndIndex = ((int[])root["indices"])[1];
			return entity;
		}

		public string ScreenName { get; private set; }
		public string Name { get; private set; }
		public long UserId { get; private set; }
	}
}