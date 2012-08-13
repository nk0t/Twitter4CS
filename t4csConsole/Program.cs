﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Twitter4CS;
using Twitter4CS.Authentication;
using System.Diagnostics;
using Twitter4CS.Rest;

namespace t4csConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			OAuth account = new Account();
			string token;
			Uri url = account.GetAuthorizeUrl(out token);
			Process.Start(url.ToString());

			string pin = Console.ReadLine();
			long userId = 0;
			string screenName = string.Empty;
			if (account.GetAccessToken(token, pin, out userId, out screenName))
			{
				Console.WriteLine("認証成功");
				TwitterApi.GetStatus(account, 234837736115281920);
			}
			else
			{
				Console.WriteLine("認証失敗");
				return;
			}

			while (true)
			{
				var s = Console.ReadLine();
				if (s == "")
					break;
				TwitterApi.UpdateStatus(account, s);
				//account.UpdateStatus(s); こっちでもいいです
			}
		}
	}
}
