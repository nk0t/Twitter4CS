using System;
using Twitter4CS.Util;

namespace Twitter4CS
{
	public class DirectMessage
	{
		private DirectMessage()
		{
		}

		public static DirectMessage Create(dynamic root)
		{
			if (root == null)
				throw new ArgumentNullException();
			var message = new DirectMessage();
			message.Id = (long)root["id"];
			message.Text = ((string)root["text"]).ParseString();
			message.Sender = User.Create(root["sender"]);
			message.SenderId = (long)root["sender_id"];
			message.SenderScreenName = root["sender_screen_name"];
			message.Recipient = User.Create(root["recipient"]);
			message.RecipientId = (long)root["recipient_id"];
			message.RecipientScreenName = root["recipient_screen_name"];
			message.CreatedAt = ((string)root["created_at"]).ToDateTime();
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

		public override bool Equals(object obj)
		{
			if (obj is DirectMessage)
				return Id == ((DirectMessage)obj).Id;
			else
				return false;
		}

		public override string ToString()
		{
			return string.Format("{0}:{1} [{2}]", SenderScreenName, Text, Id);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
