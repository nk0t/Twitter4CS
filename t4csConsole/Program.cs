using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Twitter4CS;
using Twitter4CS.Authentication;
using System.Diagnostics;

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

			Dictionary<string, string> para = new Dictionary<string, string>();
			string a = Console.ReadLine();
			para.Add("status", OAuth.UrlEncode(a));
			Console.WriteLine(account.Post("http://api.twitter.com/1/statuses/update.xml", para));
			// statusにポストするツイート指定してAPIたたくだけ
			// あとで実装する

			//まだいろいろと実装中だからpostもまともにできないす
		}
	}
}
