using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class Relationship
	{
		private Relationship()
		{
		}

		public static Relationship Create(XElement node)
		{
			if (node == null)
				throw new ArgumentNullException();
			var relation = new Relationship();
			var sourceNode = node.Element("source");
			var targetNode = node.Element("target");
			relation.SourceId = sourceNode.Element("id").Value.ToLong();
			relation.SourceScreenName = sourceNode.Element("screen_name").Value;
			relation.TargetId = targetNode.Element("id").Value.ToLong();
			relation.TargetScreenName = targetNode.Element("screen_name").Value;
			relation.IsSourceFollowingTarget = sourceNode.Element("following").Value.ToBool();
			relation.IsSourceFollowedByTarget = sourceNode.Element("followed_by").Value.ToBool();
			relation.IsSourceBlockingTarget = sourceNode.Element("blocking").Value == null;
			relation.IsSourceNotificationEnabled = sourceNode.Element("notifications_enabled").Value == null;
			return relation;
		}

		public static Relationship Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var relation = new Relationship();
			root = root.relationship;
			var source = root.source;
			var target = root.target;
			relation.SourceId = (long)source.id;
			relation.SourceScreenName = source.screen_name;
			relation.TargetId = (long)target.id;
			relation.TargetScreenName = target.screen_name;
			relation.IsSourceFollowingTarget = (bool)source.following;
			relation.IsSourceFollowedByTarget = (bool)source.followed_by;
			relation.IsSourceBlockingTarget = source.blocking == "null";
			relation.IsSourceNotificationEnabled = source.notifications_enabled == "null";
			return relation;
		}

		public long SourceId { get; private set; }
		public string SourceScreenName { get; private set; }
		public long TargetId { get; private set; }
		public string TargetScreenName { get; private set; }
		public bool IsSourceFollowedByTarget { get; private set; }
		public bool IsSourceFollowingTarget { get; private set; }
		public bool IsSourceBlockingTarget { get; private set; }
		public bool IsSourceNotificationEnabled { get; private set; }
	}
}
