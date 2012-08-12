using System;
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
			}
			else
			{
				Console.WriteLine("認証失敗");
				return;
			}

			var s = Console.ReadLine();
			TwitterApi.UpdateStatus(account, s);

			//まだいろいろと実装中だからpostもまともにできないす
		}
	}
}
