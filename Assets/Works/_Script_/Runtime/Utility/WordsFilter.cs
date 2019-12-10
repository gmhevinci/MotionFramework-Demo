using System;
using System.Collections;
using System.Collections.Generic;

public static class WordsFilter
{
	/// <summary>
	/// 敏感字列表
	/// </summary>
	private static readonly List<string> _filterWords = new List<string>(10000);


	/// <summary>
	/// 清空敏感字列表
	/// </summary>
	public static void ClearFilterWords()
	{
		_filterWords.Clear();
	}

	/// <summary>
	/// 添加敏感字
	/// </summary>
	public static void AddFilterWords(string word)
	{
		_filterWords.Add(word);
	}

	/// <summary>
	/// 检测敏感字
	/// </summary>
	/// <param name="content">欲过滤的内容</param>
	/// <param name="strResult">执行过滤之后的内容</param>
	/// <param name="filterDeep">检测深度。即敏感字数组中的每个词中的插入几个字以内会被过滤掉，例：检测深度为2，敏感字里有个词是中国，那么“中国”、“中*国”，“中**国”都会被过滤掉（*是任意字）。</param>
	/// <param name="checkOnly">是否只检测而不执行过滤操作</param>
	/// <param name="isTrim">过滤之前是否要去掉头尾的空字符</param>
	/// <param name="strReplace">将检测到的敏感字替换成的字符</param>
	/// <returns>如果包含敏感字返回TRUE</returns>
	public static bool FilterWords(string content, out string strResult, int filterDeep = 0, bool checkOnly = false, bool isTrim = true, string strReplace = "*")
	{
		string result = content;
		if (isTrim)
			result = result.Trim();

		strResult = result;
		bool check = false;
		foreach (string str in _filterWords)
		{
			string s = str.Replace(strReplace, "");
			if (s.Length == 0)
				continue;

			bool bFiltered = true;
			while (bFiltered)
			{
				int result_index_start = -1;
				int result_index_end = -1;
				int idx = 0;
				while (idx < s.Length)
				{
					string one_s = s.Substring(idx, 1);
					if (one_s == strReplace)
					{
						continue;
					}
					if (result_index_end + 1 >= result.Length)
					{
						bFiltered = false;
						break;
					}
					int new_index = result.IndexOf(one_s, result_index_end + 1, StringComparison.OrdinalIgnoreCase);
					if (new_index == -1)
					{
						bFiltered = false;
						break;
					}
					if (idx > 0 && new_index - result_index_end > filterDeep + 1)
					{
						bFiltered = false;
						break;
					}
					result_index_end = new_index;

					if (result_index_start == -1)
					{
						result_index_start = new_index;
					}
					idx++;
				}

				if (bFiltered)
				{
					if (checkOnly)
						return true;

					check = true;
					string result_left = result.Substring(0, result_index_start);
					for (int i = result_index_start; i <= result_index_end; i++)
					{
						result_left += strReplace;
					}
					string result_right = result.Substring(result_index_end + 1);
					result = result_left + result_right;
				}
			}
		}

		strResult = result;
		return check;
	}
}