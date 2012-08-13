﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class HashTagEntity : Entity
	{
		private HashTagEntity()
		{
		}

		public static HashTagEntity Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var entity = new HashTagEntity();
			entity.Text = (string)node.Element("text").Value;
			entity.StartIndex = ((string)node.Attribute("start").Value).ToInteger();
			entity.EndIndex = ((string)node.Attribute("end").Value).ToInteger();
			return entity;
		}

		public string Text { get; private set; }
		public int StartIndex { get; private set; }
		public int EndIndex { get; private set; }
	}
}