using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Twitter4CS;
using Twitter4CS.Authentication;

namespace t4csConsole
{
	public class Account : OAuth
	{
		public Account() : base() { }

		public Account(string token, string secret) : base(token, secret) { }

		protected override string ConsumerKey
		{
			get
			{
				return "ufEUK2vLIheFZEcjeaEuA";
			}
		}

		protected override string ConsumerSecret
		{
			get
			{
				return "E46TgA1U6l6LdVAIs2z13xPRkgQLnQj407kBURi55g";
			}
		}
	}
}
