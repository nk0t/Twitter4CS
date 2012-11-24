﻿using System;

namespace Twitter4CS
{
	/// <summary>
	/// ハッシュタグ要素を表します
	/// </summary>
	public class HashtagEntity : Entity
	{
		private HashtagEntity()
		{
		}

		public static HashtagEntity Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var entity = new HashtagEntity();
			entity.Text = root["text"];
			entity.StartIndex = ((int[])root["indices"])[0];
			entity.EndIndex = ((int[])root["indices"])[1];
			return entity;
		}

		public string Text { get; private set; }
	}
}