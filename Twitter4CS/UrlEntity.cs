﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class UrlEntity : Entity
	{
		private UrlEntity()
		{
		}

		public static UrlEntity Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var entity = new UrlEntity();
			entity.Url = (string)node.Element("expanded_url").Value;
			entity.ShortenUrl = (string)node.Element("url").Value;
			entity.DisplayUrl = (string)node.Element("display_url").Value;
			entity.StartIndex = ((string)node.Attribute("start").Value).ToInteger();
			entity.EndIndex = ((string)node.Attribute("end").Value).ToInteger();
			return entity;
		}

		public string Url { get; private set; }
		public string ShortenUrl { get; private set; }
		public string DisplayUrl { get; private set; }
		public int StartIndex { get; private set; }
		public int EndIndex { get; private set; }
	}
}