﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Twitter4CS;
using Twitter4CS.Authentication;
using System.Diagnostics;
using Twitter4CS.Rest;
using System.IO;

namespace t4csConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			OAuth account;

			string token = "";
			string secret = "";
			if (File.Exists("token.txt"))
			{
				string[] ts = File.ReadAllText("token.txt").Split(':');
				token = ts[0];
				secret = ts[1];
				account = new Account(ts[0], ts[1]);
				Console.WriteLine("認証完了");
			}
			else
			{
				account = new Account();
				Uri url = account.GetAuthorizeUrl(out token);
				Process.Start(url.ToString());

				string pin = Console.ReadLine();
				long userId = 0;
				string screenName = string.Empty;
				if (account.GetAccessToken(token, pin, out userId, out screenName))
				{
					Console.WriteLine("認証成功");
					if (!File.Exists("token.txt"))
					{
						File.WriteAllText("token.txt", string.Format("{0}:{1}", account.Token, account.Secret));
					}
				}
				else
				{
					Console.WriteLine("認証失敗");
					return;
				}
			}

			while (true)
			{
				var s = Console.ReadLine();
				if (s == "")
					break;
				var obj = TwitterApi.GetHomeTimeline(account);
				//account.UpdateStatus(s); こっちでもいいです
				foreach(var v in obj)
					Console.WriteLine(string.Format("{0}", v));
			}
		}
	}
}
