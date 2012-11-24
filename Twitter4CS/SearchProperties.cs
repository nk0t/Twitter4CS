using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twitter4CS
{
	/// <summary>
	/// 検索条件を表します
	/// </summary>
	public class SearchProperties
	{
		public SearchProperties()
		{
			Lang = "ja";
			ResultType = SearchResultType.mixed;
			SinceId = -1;
			MaxId = -1;
			Count = 100;
			Until = new DateTime(0);
		}
		/// <summary>
		/// 検索する言語を取得/設定します(デフォルトではja)
		/// </summary>
		public string Lang { get; set; }
		/// <summary>
		/// レスポンス形式を取得/設定します(デフォルトではrecent)\n
		/// 詳細はhttps://dev.twitter.com/docs/api/1.1/get/search/tweetsを参照してください
		/// </summary>
		public SearchResultType ResultType { get; set; }
		/// <summary>
		/// 設定した日時以前のツイートを検索します(デフォルト値なし)
		/// </summary>
		public DateTime Until { get; set; }
		/// <summary>
		/// 設定したStatusIdよりも新しいツイートを検索します(デフォルト値なし)
		/// </summary>
		public long SinceId { get; set; }
		/// <summary>
		/// 設定したStatusId以前のツイートを検索します(デフォルト値なし)
		/// </summary>
		public long MaxId { get; set; }
		/// <summary>
		/// 取得する件数を取得/設定します
		/// </summary>
		public int Count { get; set; }
	}

	public enum SearchResultType
	{
		mixed,
		recent,
		popular
	}
}
