﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class UserMentionEntity : Entity
	{
		private UserMentionEntity()
		{
		}

		public static UserMentionEntity Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var entity = new UserMentionEntity();
			entity.ScreenName = node.Element("screen_name").Value;
			entity.Name = node.Element("name").Value;
			entity.UserId = node.Element("id").Value;
			entity.StartIndex = node.Attribute("start").Value.ToInteger();
			entity.EndIndex = node.Attribute("end").Value.ToInteger();
			return entity;
		}

		public string ScreenName { get; private set; }
		public string Name { get; private set; }
		public string UserId { get; private set; }
	}
}