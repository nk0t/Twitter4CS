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
