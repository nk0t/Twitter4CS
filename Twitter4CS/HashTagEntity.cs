﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class HashtagEntity : Entity
	{
		private HashtagEntity()
		{
		}

		public static HashtagEntity Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var entity = new HashtagEntity();
			entity.Text = node.Element("text").Value;
			entity.StartIndex = node.Attribute("start").Value.ToInteger();
			entity.EndIndex = node.Attribute("end").Value.ToInteger();
			return entity;
		}
		public string Text { get; private set; }
	}
}