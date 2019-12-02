using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	/// <summary>
	/// 多语言接口类
	/// </summary>
	public static class LANG
	{
		public static string Convert(int hashCode)
		{
			var tab = CfgAutoGenerateLanguage.Instance.GetCfgTab(hashCode);
			if (tab != null)
				return tab.Lang;
			else
				return string.Empty;
		}
	}
}