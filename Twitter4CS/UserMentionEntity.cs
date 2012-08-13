using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
    public class UserMentionEntity
    {
        private UserMentionEntity()
        {
        }

        public static UserMentionEntity Create(XElement node)
        {
            if(node == null)
                throw new ArgumentNullException();
            var entity = new UserMentionEntity();
            entity.ScreenName= (string)node.Element("screen_name").Value;
            entity.Name = (string)node.Element("name").Value;
            entity.UserId = (string)node.Element("id").Value;
            entity.StartIndex = ((string)node.Attribute("start").Value).ToInteger();
            entity.EndIndex = ((string)node.Attribute("end").Value).ToInteger();
            return entity;
        }

        public string ScreenName { get; private set; }
        public string Name { get; private set; }
        public string UserId { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
    }
}
