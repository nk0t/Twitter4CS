﻿using System;

namespace Twitter4CS
{
	public class UrlEntity : Entity
	{
		private UrlEntity()
		{
		}

		public static UrlEntity Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var entity = new UrlEntity();
			entity.Url = root["expanded_url"];
			entity.ShortenUrl = root["url"];
			entity.DisplayUrl = root["display_url"];
			entity.StartIndex = ((int[])root["indices"])[0];
			entity.EndIndex = ((int[])root["indices"])[1];
			return entity;
		}

		public string Url { get; private set; }
		public string ShortenUrl { get; private set; }
		public string DisplayUrl { get; private set; }

	}
}