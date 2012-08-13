﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Twitter4CS.Util;

namespace Twitter4CS
{
    public class DirectMessage
    {
        private DirectMessage()
        {
        }

        public static DirectMessage Create(XElement node)
        {
            if (node == null)
                throw new ArgumentNullException();
            var message = new DirectMessage();
            message.Id = node.Element("id").Value.ToLong();
            message.Text = node.Element("text").ParseString();
            message.Sender = User.Create(node.Element("sender"));
            message.SenderId = node.Element("sender_id").Value.ToLong();
            message.SenderScreenName = node.Element("sender_screen_name").Value;
            message.Recipient = User.Create(node.Element("recipient"));
            message.RecipientId = node.Element("recipient_id").Value.ToLong();
            message.RecipientScreenName = node.Element("recipient_screen_name").Value;
            message.CreatedAt = node.Element("created_at").Value.ToDateTime();
            return message;
        }

        public long Id { get; private set; }
        public string Text { get; private set; }
        public User Sender { get; private set; }
        public long SenderId { get; private set; }
        public string SenderScreenName { get; private set; }
        public User Recipient { get; private set;}
        public long RecipientId { get; private set; }
        public string RecipientScreenName { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
